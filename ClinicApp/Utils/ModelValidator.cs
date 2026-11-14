using System.Reflection;
using ClinicApp.Attributes;
namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();
        PropertyInfo[] properties = obj.GetType().GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            object? value = prop.GetValue(obj);

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null)
            {
                if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                {
                    result.AddError($"{prop.Name}: {required.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null)
            {
                if (value is string text && text.Length > maxLength.Length)
                {
                    result.AddError($"{prop.Name}: {maxLength.ErrorMessage}");
                }
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null)
            {
                if (value != null && Convert.ToDouble(value) < minValue.Min)
                {
                    result.AddError($"{prop.Name}: {minValue.ErrorMessage}");
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine($"Тип: {type.Name}");
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            string line = $"  {prop.Name} ({prop.PropertyType.Name})";

            foreach (Attribute attr in prop.GetCustomAttributes())
            {
                line += $" [{attr.GetType().Name.Replace("Attribute", "")}]";
            }

            Console.WriteLine(line);
        }
    }
}