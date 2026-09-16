namespace Lab01;

internal class Program
{
    private static void Main(string[] args)
    {
        // Додайте цей рядок для підтримки української мови у консолі:
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        Task1.Run();
    }
}