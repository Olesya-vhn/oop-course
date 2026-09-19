namespace Lab01;

public static class Task6
{
    public static void Run()
    {
        Console.Write("Введіть номер студентського квитка: ");
        int studentId = int.Parse(Console.ReadLine()!);

        int lastDigit = studentId % 10;
        string faculty;

        if (lastDigit == 0 || lastDigit == 1)
        {
            faculty = "комп'ютерних наук";
        }
        else if (lastDigit == 2 || lastDigit == 3)
        {
            faculty = "економічний";
        }
        else if (lastDigit == 4 || lastDigit == 5)
        {
            faculty = "інженерний";
        }
        else if (lastDigit == 6 || lastDigit == 7)
        {
            faculty = "філологічний";
        }
        else
        {
            faculty = "юридичний";
        }

        string isFullTime;
        if (studentId % 2 == 0)
        {
            isFullTime = "так";
        }
        else
        {
            isFullTime = "ні";
        }

        string hasStipend;
        if (studentId % 3 == 0)
        {
            hasStipend = "так";
        }
        else
        {
            hasStipend = "ні";
        }

        Console.WriteLine($"Факультет:   {faculty}");
        Console.WriteLine($"Денна форма: {isFullTime}");
        Console.WriteLine($"Стипендія:   {hasStipend}");
    }
}