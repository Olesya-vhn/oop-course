namespace Lab01;

public static class Task5
{
    public static void Run()
    {
        Console.Write("Введіть код рівня (B, M або P): ");
        string code = Console.ReadLine()!.ToUpper();

        string result;

        switch (code)
        {
            case "B":
                result = "Рівень: Бакалавр";
                break;
            case "M":
                result = "Рівень: Магістр";
                break;
            case "P":
                result = "Рівень: Аспірант";
                break;
            default:
                result = "Рівень: невідомий рівень";
                break;
        }

        Console.WriteLine(result);
    }
}