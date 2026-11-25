using ClinicApp.Models;
namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Appointment[] all = _appointments.GetAll();

        return _doctors.GetAll().Select(d =>
        {
            Appointment[] mine = all.Where(a => a.DoctorId == d.Id).ToArray();
            DateTime last = mine.Any() ? mine.Max(a => a.ScheduledAt) : DateTime.MinValue;
            return new DoctorStats(d.Id, d.FullName, mine.Count(), mine.Sum(a => a.GetCost()), last);
        });
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Appointment[] all = _appointments.GetAll();

        return _patients.GetAll().Select(p =>
        {
            Appointment[] mine = all.Where(a => a.PatientId == p.Id).ToArray();
            DateTime last = mine.Any() ? mine.Max(a => a.ScheduledAt) : DateTime.MinValue;
            return new PatientStats(p.Id, p.FullName, mine.Count(), mine.Sum(a => a.GetCost()), last);
        });
    }
}