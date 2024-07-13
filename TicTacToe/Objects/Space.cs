namespace TicTacToe.Objects;

public class Space
{
    public (int x, int y) Location { get; set; }
    public bool Selected { get; set; }
    public bool Occupied { get; set; }
    public bool Player { get; set; }
    public bool WinningPlay { get; set; }
}