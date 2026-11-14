using System.Reflection;
using ClinicApp.Attributes;
namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();
        PropertyInfo[] properties = typeof(T).GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];

            if (prop.GetSetMethod() == null)
            {
                continue;
            }

            string hint = BuildHint(prop);

            while (true)
            {
                Console.Write($"  {prop.Name}{hint}: ");
                string input = Console.ReadLine() ?? "";

                try
                {
                    object converted = Convert.ChangeType(input, prop.PropertyType);
                    prop.SetValue(obj, converted);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }

    private static string BuildHint(PropertyInfo prop)
    {
        string hint = "";

        if (prop.GetCustomAttribute<RequiredAttribute>() != null)
        {
            hint += "обов'язкове";
        }

        MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLength != null)
        {
            if (hint.Length > 0) hint += ", ";
            hint += $"макс. {maxLength.Length}";
        }

        MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
        if (minValue != null)
        {
            if (hint.Length > 0) hint += ", ";
            hint += $"мін. {minValue.Min}";
        }

        if (hint.Length == 0)
        {
            return "";
        }
        return $" ({hint})";
    }
}