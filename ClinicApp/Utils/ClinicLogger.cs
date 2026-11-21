using System.Text;
using ClinicApp.Events;
namespace ClinicApp.Utils;

public class ClinicLogger
{
    private string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        Write("INFO ", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN ", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (!File.Exists(_logPath) || n <= 0)
        {
            return new string[0];
        }

        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skip = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - skip];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = lines[skip + i];
        }
        return result;
    }

    public void Clear()
    {
        if (File.Exists(_logPath))
        {
            File.Delete(_logPath);
        }
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void Write(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }
    public void OnPatientAdded(object? sender, PatientEventArgs e) =>
    LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}");

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} скасовано" + (e.Notes.Length > 0 ? $". Причина: {e.Notes}" : ""));

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} завершено");

    public void OnPaymentReceived(object? sender, PaymentEventArgs e) =>
        LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) =>
        LogInfo($"План лікування #{e.PlanId} завершено: пацієнт #{e.PatientId}, {e.Diagnosis}");

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}");

        Directory.CreateDirectory("alerts");
        string path = Path.Combine("alerts", $"urgent_{DateTime.Now:yyyy-MM-dd}.txt");
        string line = $"[{DateTime.Now:HH:mm:ss}] Запис #{e.AppointmentId}: пацієнт #{e.PatientId} → лікар #{e.DoctorId}, {e.ScheduledAt:dd.MM.yyyy HH:mm} | {e.Notes}{Environment.NewLine}";
        File.AppendAllText(path, line, Encoding.UTF8);
    }
}