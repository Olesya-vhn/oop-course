using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;
namespace ClinicApp.Utils;

public class ClinicExporter
{
    private Clinic _clinic;
    private string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    public string ExportPatients()
    {
        string path = PreparePath("patients.txt");
        Patient[] patients = _clinic.Patients.GetAll();

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Пацієнти");
            for (int i = 0; i < patients.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {patients[i]}");
            }
            WriteFooter(writer, "Всього", patients.Length);
        }
        return path;
    }

    public string ExportAppointments()
    {
        string path = PreparePath("appointments.txt");
        Appointment[] appointments = _clinic.Appointments.GetAll();

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Записи на прийом");
            for (int i = 0; i < appointments.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {appointments[i]}");
            }
            WriteFooter(writer, "Всього", appointments.Length);
        }
        return path;
    }

    public string ExportBilling()
    {
        string path = PreparePath("billing.txt");
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Рахунки (неоплачені записи)");
            for (int i = 0; i < unpaid.Length; i++)
            {
                writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
            }
            WriteFooter(writer, "Неоплачених", unpaid.Length);
            writer.WriteLine($"Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн");
        }
        return path;
    }

    public string ExportTreatmentPlans()
    {
        string path = PreparePath("treatment_plans.txt");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Плани лікування");
            for (int i = 0; i < plans.Length; i++)
            {
                TreatmentPlan p = plans[i];
                writer.WriteLine($"{i + 1,3}. [{p.Id}] Пацієнт #{p.PatientId} | {p.Diagnosis} | {p.DurationDays} дн. | {p.Status}");
            }
            WriteFooter(writer, "Всього", plans.Length);
        }
        return path;
    }

    public string[] ExportAll()
    {
        return new string[]
        {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }

    private string PreparePath(string fileName)
    {
        string dir = Path.Combine(_baseDir, DateTime.Today.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, fileName);
    }

    private void WriteHeader(StreamWriter writer, string title)
    {
        writer.WriteLine($"=== {title} — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));
    }

    private void WriteFooter(StreamWriter writer, string label, int count)
    {
        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"{label}: {count}");
    }
}