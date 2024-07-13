using CheckersGame.Assets;
using TicTacToe.Models;

namespace TicTacToe.Objects;

public class Board
{
    private readonly string _first;
    private readonly Scoreboard _scoreboard;
    private readonly List<List<Space>> _spaces;
    private (int x, int y) _cursor = (1, 1);
    private bool _gameOver = false;

    public Board(Scoreboard scoreboard, bool playerFirst)
    {
        _scoreboard = scoreboard;
        _first = playerFirst ? "Player" : "CPU";
        _spaces = new List<List<Space>>();
        for (var x = 0; x < 3; x++)
        {
            _spaces.Add([]);
            for (var y = 0; y < 3; y++)
                _spaces[x].Add(new Space
                {
                    Location = (y, x),
                    Occupied = false,
                    Player = false
                });
        }

        _spaces[_cursor.y][_cursor.x].Selected = true;
    }

    public void Draw()
    {
        Console.Clear();
        Console.WriteLine("TIC-TAC-TOE  WORLD CHAMPIONSHIP");
        Console.WriteLine($"-=-= YOU  [{_scoreboard.Wins}]     [{_scoreboard.Losses}]  CPU =-=-");
        var alt = false;
        foreach (var row in _spaces)
        {
            Console.Write("-=-=-=     ");
            foreach (var space in row)
            {
                Console.BackgroundColor =
                    space.WinningPlay ? ConsoleColor.Cyan : alt ? ConsoleColor.Gray : ConsoleColor.White;
                alt = !alt;
                DrawSpace(space);
            }

            Console.Write("     =-=-=-");
            Console.WriteLine();
        }

        Console.ResetColor();
        Console.WriteLine($"{_first} goes first.");
    }

    private void DrawSpace(Space space)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        var format = space.Selected ? "[{0}]" : " {0} ";
        var piece = " ";
        if (space.Occupied)
            piece = space.Player ? "O" : "X";

        Console.Write(format, piece);
        Console.ResetColor();
    }

    public void MoveSelector(Movement move)
    {
        var destination = (_cursor.x, _cursor.y);
        switch (move)
        {
            case Movement.Up:
                destination.y--;
                break;
            case Movement.Down:
                destination.y++;
                break;
            case Movement.Right:
                destination.x++;
                break;
            case Movement.Left:
                destination.x--;
                break;
        }

        SetCurrentSpace(destination);
    }

    public ActionResults TakeSpace(bool player, (int x, int y)? point = null)
    {
        var space =
            point != null
                ? _spaces[point.GetValueOrDefault().y][point.GetValueOrDefault().x]
                : _spaces[_cursor.y][_cursor.x];

        if (space.Occupied) return ActionResults.Invalid;

        space.Occupied = true;
        space.Player = player;

        return ActionResults.Success;
    }

    public List<List<Space>> GetAllSpaces()
    {
        return _spaces;
    }

    public List<Space> GetSpaces(List<(int x, int y)> pointers)
    {
        List<Space> results = [];
        results.AddRange(pointers.Select(point => _spaces[point.y][point.x]));
        return results;
    }

    public List<Space> GetRemainingSpaces()
    {
        List<Space> results = [];
        foreach (var row in _spaces)
            results.AddRange(row.Where(s => s.Occupied == false));
        return results;
    }

    private Space GetSelectedSpace()
    {
        return _spaces[_cursor.y][_cursor.x];
    }

    private void SetCurrentSpace((int x, int y) newSpace)
    {
        var currentSpace = GetSelectedSpace();
        currentSpace.Selected = false;

        newSpace.x = int.Min(newSpace.x, _spaces[0].Count - 1);
        newSpace.x = int.Max(newSpace.x, 0);

        newSpace.y = int.Min(newSpace.y, _spaces.Count - 1);
        newSpace.y = int.Max(newSpace.y, 0);

        _spaces[newSpace.y][newSpace.x].Selected = true;
        _cursor = (newSpace.x, newSpace.y);
    }
}