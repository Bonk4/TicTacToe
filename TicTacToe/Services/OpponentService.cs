using CheckersGame.Assets;
using TicTacToe.Objects;

namespace TicTacToe.Services;

public class OpponentService
{
    private readonly Random _rng = new();
    private List<(int x, int y)> _objective = [];

    public (int x, int y) GetNextAction(List<List<Space>> board)
    {
        SetObjective(board);
        var location = _rng.Next(_objective.Count);
        var space = _objective[location];
        var failsafe = 0;
        while (board[space.y][space.x].Occupied)
        {
            var newLocation = _rng.Next(_objective.Count);
            space = _objective[newLocation];

            if (failsafe > 3) // rng sucks sometimes
                space = GetFirstAvailableSpace(board);

            failsafe++;
        }

        return space;
    }

    private (int x, int y) GetFirstAvailableSpace(List<List<Space>> board)
    {
        foreach (var row in board)
        foreach (var space in row)
            if (!space.Occupied)
                return space.Location;

        throw new Exception("No spaces available.");
    }

    private void SetObjective(List<List<Space>> board)
    {
        // check if current strategy is still viable
        if (StrategyStillViable(board)) return;

        // try to find a new strategy
        var targets = GameAssets.WinningCombos.Find(row =>
            row.All(space => !board[space.y][space.x].Occupied || !board[space.y][space.x].Player == false));

        if (targets != null)
        {
            _objective = targets;
        }
        else
        {
            // if no wins are available, just target any remaining spaces
            var spaces = board.SelectMany(x => x.FindAll(space => !space.Occupied && !space.Player));
            var remainingSpaces = spaces.Select(space => new ValueTuple<int, int>(space.Location.x, space.Location.y))
                .ToList();
            _objective = remainingSpaces;
        }
    }

    private bool StrategyStillViable(List<List<Space>> board)
    {
        if (_objective.Count <= 0) return false;

        foreach (var point in _objective)
        {
            var space = board[point.y][point.x];
            if (space is { Occupied: true, Player: true }) return false;
        }

        return true;
    }
}