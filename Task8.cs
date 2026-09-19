namespace Lab01;

public static class Task8
{
    public static void Run()
    {
        Console.Write("Введіть суму балів: ");
        double totalPoints = double.Parse(Console.ReadLine()!);

        Console.Write("Введіть кількість предметів: ");
        int subjectsCount = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть ціну за кредит: ");
        double pricePerCredit = double.Parse(Console.ReadLine()!);

        Console.Write("Введіть кількість кредитів: ");
        int credits = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть знижку (%): ");
        int discount = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть кредити курсу: ");
        int courseCredits = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть бал за екзамен: ");
        int score = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть кількість спроб: ");
        int attempts = int.Parse(Console.ReadLine()!);

        double avgScore = CalculateAverageScore(totalPoints, subjectsCount);
        string scoreCategory = GetScoreCategory(avgScore);
        Console.WriteLine($"Середній бал: {avgScore:F2} -> {scoreCategory}");

        double totalCost = CalculateCost(pricePerCredit, credits, discount);
        Console.WriteLine($"Вартість: {totalCost:F2} грн");

        string level = GetCourseLevel(courseCredits);
        Console.WriteLine($"Кредити: {courseCredits}, рівень: {level}");

        string assessment = GetAssessmentStatus(score, attempts);
        Console.WriteLine($"Результат: {score} балів ({attempts} спроба) — {assessment}");
    }

    public static double CalculateAverageScore(double totalPoints, int count)
    {
        return totalPoints / count;
    }

    public static string GetScoreCategory(double avgScore)
    {
        if (avgScore < 60.0) return "незадовільно";
        if (avgScore < 75.0) return "задовільно";
        if (avgScore < 90.0) return "добре";
        return "відмінно";
    }

    public static double CalculateCost(double pricePerCredit, int credits, int discount)
    {
        return pricePerCredit * credits * (1.0 - discount / 100.0);
    }

    public static string GetCourseLevel(int credits)
    {
        if (credits <= 3) return "базовий";
        if (credits <= 6) return "середній";
        return "поглиблений";
    }

    public static string GetAssessmentStatus(int score, int attempts)
    {
        if (score >= 90) return "відмінно";
        if (score >= 75 && attempts == 1) return "добре";
        if (score >= 60 || attempts > 1) return "задовільно";
        return "незадовільно";
    }
}