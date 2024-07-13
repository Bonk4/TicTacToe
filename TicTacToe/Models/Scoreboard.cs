namespace TicTacToe.Models;

public class Scoreboard
{
    public Scoreboard()
    {
        Wins = 0;
        Losses = 0;
    }

    public int Wins { get; set; }
    public int Losses { get; set; }
}