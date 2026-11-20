using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;
namespace ClinicApp.Utils;

public class SessionManager
{
    private string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        Patient[] patients = clinic.Patients.GetAll();

        using (StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8))
        {
            writer.WriteLine("[PATIENTS]");
            for (int i = 0; i < patients.Length; i++)
            {
                Patient p = patients[i];
                writer.WriteLine($"{p.FirstName},{p.LastName},{p.BirthDate:dd.MM.yyyy},{p.BloodType},{p.Phone}");
            }
            writer.WriteLine("[END]");
        }
    }

    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath))
        {
            return 0;
        }

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string section = "";
        int added = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (line.Length == 0)
            {
                continue;
            }

            if (line.StartsWith("["))
            {
                section = line;
                continue;
            }

            if (section == "[PATIENTS]")
            {
                if (TryLoadPatient(clinic, line))
                {
                    added++;
                }
            }
        }

        return added;
    }

    private bool TryLoadPatient(Clinic clinic, string line)
    {
        try
        {
            string[] parts = line.Split(',');
            if (parts.Length != 5)
            {
                return false;
            }

            string firstName = parts[0].Trim();
            string lastName = parts[1].Trim();
            DateTime birthDate = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
            BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
            string phone = parts[4].Trim();

            if (PatientExists(clinic, firstName, lastName, birthDate))
            {
                return false;
            }

            clinic.Patients.Add(new Patient(firstName, lastName, birthDate, bloodType, phone));
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private bool PatientExists(Clinic clinic, string firstName, string lastName, DateTime birthDate)
    {
        Patient[] existing = clinic.Patients.GetAll();
        for (int i = 0; i < existing.Length; i++)
        {
            if (existing[i].FirstName == firstName
                && existing[i].LastName == lastName
                && existing[i].BirthDate.Date == birthDate.Date)
            {
                return true;
            }
        }
        return false;
    }
}