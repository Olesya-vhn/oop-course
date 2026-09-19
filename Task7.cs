namespace Lab01;

public static class Task7
{
    public static void Run()
    {
        Console.Write("Введіть кількість предметів: ");
        int count = int.Parse(Console.ReadLine()!);

        decimal[] grades = new decimal[count];

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Введіть оцінку #{i + 1}: ");
            grades[i] = decimal.Parse(Console.ReadLine()!);
        }

        decimal sum = 0m;
        decimal min = grades[0];
        decimal max = grades[0];

        for (int i = 0; i < count; i++)
        {
            sum += grades[i];

            if (grades[i] < min)
            {
                min = grades[i];
            }

            if (grades[i] > max)
            {
                max = grades[i];
            }
        }

        decimal average = sum / count;

        int aboveAverageCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (grades[i] > average)
            {
                aboveAverageCount++;
            }
        }

        int excellentIndex = -1;
        int index = 0;
        while (index < count)
        {
            if (grades[index] >= 90m)
            {
                excellentIndex = index;
                break;
            }
            index++;
        }

        Console.WriteLine("=== Звіт по успішності ===");
        Console.WriteLine($"Кількість дисциплін: {count}");
        Console.WriteLine($"Загальна сума балів: {sum:F2}");
        Console.WriteLine($"Середній бал:        {average:F2}");
        Console.WriteLine($"Мін / Макс:          {min:F2} / {max:F2}");
        Console.WriteLine($"Вище середнього:     {aboveAverageCount} з {count}");

        if (excellentIndex != -1)
        {
            Console.WriteLine($"Перша відмінна (>=90): #{excellentIndex + 1} — {grades[excellentIndex]:F2} балів");
        }
        else
        {
            Console.WriteLine("Перша відмінна (>=90): немає");
        }

        Console.WriteLine("==========================");
    }
}