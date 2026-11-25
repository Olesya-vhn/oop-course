using ClinicApp.Enums;
using ClinicApp.Models;
namespace ClinicApp.Managers;

public class ReportManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;
    private AnalyticsManager _analytics;

    public ReportManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
        _analytics = new AnalyticsManager(appointments, doctors, patients);
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Appointment[] all = _appointments.GetAll();

        return _doctors.GetAll()
            .GroupBy(d => d.Speciality)
            .Select(g =>
            {
                int[] doctorIds = g.Select(d => d.Id).ToArray();
                Appointment[] mine = all.Where(a => doctorIds.Contains(a.DoctorId)).ToArray();
                return new SpecialityReport(g.Key, g.Count(), mine.Length, mine.Sum(a => a.GetCost()));
            })
            .OrderByDescending(r => r.TotalRevenue);
    }

    public string? FindBusiestDoctorName()
    {
        DoctorStats? top = _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.AppointmentCount)
            .FirstOrDefault();
        return top?.FullName;
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        return _appointments.GetAll()
            .GroupBy(a => a.PatientId)
            .Where(g => g.Count() >= minVisits)
            .Join(_patients.GetAll(), g => g.Key, p => p.Id, (g, p) => p.FullName);
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.TotalRevenue)
            .Take(n);
    }

    public bool HasAnyUrgentAppointments()
    {
        return _appointments.GetAll().Any(a => a is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        return _doctors.GetAll()
            .Select(d => d.Speciality)
            .Distinct()
            .OrderBy(s => s);
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        return _appointments.GetAll()
            .GroupBy(a => new { a.ScheduledAt.Year, a.ScheduledAt.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => (Year: g.Key.Year, Month: g.Key.Month, Total: g.Sum(a => a.GetCost())));
    }
}