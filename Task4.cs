namespace Lab01;

public static class Task4
{
    public static void Run()
    {
        Console.Write("Введіть бал: ");
        int score = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть номер спроби: ");
        int attempts = int.Parse(Console.ReadLine()!);

        string status;

        if (score >= 90)
        {
            status = "відмінно";
        }
        else if (score >= 75 && attempts == 1)
        {
            status = "добре";
        }
        else if (score >= 60 || attempts > 1)
        {
            status = "задовільно";
        }
        else
        {
            status = "незадовільно";
        }

        Console.WriteLine($"Результат: {score} балів ({attempts} спроба) — {status}");
    }
}