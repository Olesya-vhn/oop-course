namespace Lab01;

public static class Task1
{
    public static void Run()
    {
        Console.Write("Введіть загальну суму балів: ");
        double totalPoints = double.Parse(Console.ReadLine()!);

        Console.Write("Введіть кількість предметів: ");
        int subjectsCount = int.Parse(Console.ReadLine()!);

        double averageScore = totalPoints / subjectsCount;

        Console.WriteLine($"Середній бал: {averageScore:F2}");
    }
}