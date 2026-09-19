namespace Lab01;

public static class Task2
{
    public static void Run()
    {
        Console.Write("Введіть ціну за 1 кредит: ");
        double pricePerCredit = double.Parse(Console.ReadLine()!);

        Console.Write("Введіть кількість кредитів: ");
        int credits = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть відсоток знижки: ");
        int discount = int.Parse(Console.ReadLine()!);

        double totalCost = pricePerCredit * credits * (1.0 - discount / 100.0);

        Console.WriteLine($"Вартість: {totalCost:F2} грн");
    }
}