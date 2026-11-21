using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;
namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private Clinic _clinic;
    private string _dir;

    public PatientPassportWriter(Clinic clinic, string dir = "patients")
    {
        _clinic = clinic;
        _dir = dir;
        Directory.CreateDirectory(_dir);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => WritePassport(e.PatientId);

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => WritePassport(e.PatientId);

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => WritePassport(e.PatientId);

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null)
        {
            return;
        }

        string path = Path.Combine(_dir, $"passport_{patientId}.txt");

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
            writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
            writer.WriteLine();

            writer.WriteLine($"Ім'я: {patient.FullName}");
            writer.WriteLine($"Дата народження: {patient.BirthDate:dd.MM.yyyy} ({ClinicFormatter.FormatAge(patient.Age)})");
            writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
            writer.WriteLine();

            MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

            writer.WriteLine("-- Діагнози --");
            int count = 0;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is Diagnosis)
                {
                    writer.WriteLine(records[i].GetSummary());
                    count++;
                }
            }
            if (count == 0) writer.WriteLine("(немає)");

            writer.WriteLine("-- Аналізи --");
            count = 0;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is LabResult)
                {
                    writer.WriteLine(records[i].GetSummary());
                    count++;
                }
            }
            if (count == 0) writer.WriteLine("(немає)");

            writer.WriteLine("-- Рецепти --");
            count = 0;
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i] is Prescription)
                {
                    writer.WriteLine(records[i].GetSummary());
                    count++;
                }
            }
            if (count == 0) writer.WriteLine("(немає)");
            writer.WriteLine();

            writer.WriteLine("-- Записи --");
            Appointment[] appointments = _clinic.Appointments.GetByPatient(patientId);
            for (int i = 0; i < appointments.Length; i++)
            {
                Appointment a = appointments[i];
                writer.WriteLine($"[{a.Id}] {a.GetDescription()} | {a.ScheduledAt:dd.MM.yyyy HH:mm} | {a.Status} | {a.GetCost():F2} грн");
            }
            if (appointments.Length == 0) writer.WriteLine("(немає)");
            writer.WriteLine();

            writer.WriteLine("-- Плани лікування --");
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
            for (int i = 0; i < plans.Length; i++)
            {
                TreatmentPlan p = plans[i];
                writer.WriteLine($"[{p.Id}] {p.Diagnosis} | {p.DurationDays} днів | {p.Status}");
            }
            if (plans.Length == 0) writer.WriteLine("(немає)");
            writer.WriteLine();

            writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
        }
    }
}