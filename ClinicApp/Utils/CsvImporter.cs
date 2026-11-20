using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;
namespace ClinicApp.Utils;

public class CsvImporter
{
    private const int ExpectedFields = 5;

    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, $"Файл не знайдено: {filePath}");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        for (int i = 1; i < lines.Length; i++)
        {
            int lineNumber = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != ExpectedFields)
                {
                    result.AddError(lineNumber, $"Рядок має {parts.Length} {FieldWord(parts.Length)} замість {ExpectedFields}.");
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();

                ClinicValidator.ValidateName(firstName, "FirstName");
                ClinicValidator.ValidateName(lastName, "LastName");

                DateTime birthDate = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                string bloodText = parts[3].Trim();
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodText);
                if (!Enum.IsDefined(typeof(BloodType), bloodType))
                {
                    throw new ArgumentException("Невідома група крові.");
                }

                string phone = parts[4].Trim();

                Patient patient = new Patient(firstName, lastName, birthDate, bloodType, phone);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (FormatException)
            {
                result.AddError(lineNumber, "Некоректна дата (очікується dd.MM.yyyy).");
            }
            catch (ArgumentException e)
            {
                result.AddError(lineNumber, e.Message);
            }
        }

        return result;
    }

    private static string FieldWord(int count)
    {
        if (count == 1) return "поле";
        if (count >= 2 && count <= 4) return "поля";
        return "полів";
    }
}