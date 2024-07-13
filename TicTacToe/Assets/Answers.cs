namespace CheckersGame.Assets;

public enum Answers
{
    Yes,
    No
}

public static class AnswersFunctions
{
    public static Answers? ToAnswer(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.Y => Answers.Yes,
            ConsoleKey.Spacebar => Answers.Yes,
            ConsoleKey.Enter => Answers.Yes,
            ConsoleKey.N => Answers.No,
            ConsoleKey.Escape => Answers.No,
            _ => null
        };
    }
}