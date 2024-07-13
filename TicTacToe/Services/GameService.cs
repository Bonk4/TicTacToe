using CheckersGame.Assets;
using TicTacToe.Models;
using TicTacToe.Objects;

namespace TicTacToe.Services;

public class GameService
{
    private readonly bool _playerWentFirst;
    private readonly Random _rng = new();
    private Board _board;
    private OpponentService _opponent;
    private bool _playerTurn;
    public bool GameOver;
    public Winner Victor = Winner.Draw;

    public GameService()
    {
        _playerTurn = _rng.Next(2) == 0;
        _playerWentFirst = _playerTurn;
    }

    public void StartNewGame(Scoreboard score)
    {
        _board = new Board(score, _playerWentFirst);
        _opponent = new OpponentService();
        _board.Draw();
    }

    public void TakeTurn()
    {
        if (_playerTurn) TakePlayerTurn();
        else TakeOpponentTurn();
        CheckForGameOver();
        _playerTurn = !_playerTurn;
        _board.Draw();
    }

    private void CheckForGameOver()
    {
        foreach (var combo in GameAssets.WinningCombos)
        {
            var spaces = _board.GetSpaces(combo);
            if (spaces.All(x => x is { Occupied: true, Player: true }))
            {
                GameOver = true;
                Victor = Winner.Player;
                spaces.ForEach(s => s.WinningPlay = true);
                return;
            }

            if (spaces.All(x => x is { Occupied: true, Player: false }))
            {
                GameOver = true;
                Victor = Winner.Computer;
                spaces.ForEach(s => s.WinningPlay = true);
                return;
            }
        }

        if (_board.GetRemainingSpaces().Count <= 0) GameOver = true;
    }

    private void TakeOpponentTurn()
    {
        // decide where to act
        var space = _opponent.GetNextAction(_board.GetAllSpaces());
        // take space
        var result = _board.TakeSpace(false, space);
        if (result != ActionResults.Success) throw new Exception("Opponent failed to take an action.");
    }

    private void TakePlayerTurn()
    {
        var turnOver = false;
        while (!turnOver)
        {
            var key = Console.ReadKey().Key;

            var direction = ToDirection(key);
            var action = ToAction(key);
            if (direction != null) _board.MoveSelector(direction.Value);
            if (action == Actions.Confirm)
            {
                var result = _board.TakeSpace(true);
                if (result == ActionResults.Success) turnOver = true;
            }

            _board.Draw();
        }
    }

    private static Movement? ToDirection(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.UpArrow => Movement.Up,
            ConsoleKey.DownArrow => Movement.Down,
            ConsoleKey.RightArrow => Movement.Right,
            ConsoleKey.LeftArrow => Movement.Left,
            _ => null
        };
    }

    private static Actions? ToAction(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.Enter => Actions.Confirm,
            ConsoleKey.Spacebar => Actions.Confirm,
            ConsoleKey.Escape => Actions.Cancel,
            ConsoleKey.Backspace => Actions.Cancel,
            _ => null
        };
    }
}