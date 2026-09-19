namespace Lab01;

public static class Task3
{
    public static void Run()
    {
        Console.Write("Введіть кількість кредитів: ");
        int credits = int.Parse(Console.ReadLine()!);

        string level;

        if (credits <= 3)
        {
            level = "базовий";
        }
        else if (credits <= 6)
        {
            level = "середній";
        }
        else
        {
            level = "поглиблений";
        }

        Console.WriteLine($"Кредити: {credits}");
        Console.WriteLine($"Рівень: {level}");
    }
}