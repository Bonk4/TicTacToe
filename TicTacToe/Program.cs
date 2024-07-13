using CheckersGame.Assets;
using TicTacToe.Models;
using TicTacToe.Services;

Console.WriteLine("Welcome to Tic-Tac-Toe!");

var score = new Scoreboard();

var answer = true;
while (answer)
{
    var game = new GameService();
    game.StartNewGame(score);

    while (!game.GameOver) game.TakeTurn();

    if (game.Victor == Winner.Player) score.Wins++;
    if (game.Victor == Winner.Computer) score.Losses++;
    Console.WriteLine(game.Victor == Winner.Draw ? "Draw!" : $"{game.Victor} wins!");
    Console.WriteLine("Play again? y/n");

    Answers? response = null;
    while (response == null) response = AnswersFunctions.ToAnswer(Console.ReadKey().Key);
    answer = response == Answers.Yes;
}