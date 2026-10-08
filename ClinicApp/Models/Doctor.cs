using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Speciality Speciality { get; set; }
    public WorkSchedule Schedule { get; set; }
    public string RoomNumber { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor(string firstName, string lastName, Speciality speciality, WorkSchedule schedule, string roomNumber)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        Schedule = schedule;
        RoomNumber = roomNumber;
    }

    public Doctor(string firstName, string lastName)
        : this(firstName, lastName, Speciality.General, new WorkSchedule(8, 17), "100")
    {
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public bool CanAcceptAt(DateTime dateTime)
    {
        return Schedule.Contains(dateTime.Hour);
    }

    public override string ToString()
    {
        string specialityFormatted = ClinicFormatter.FormatSpeciality(Speciality);
        return $"[{Id}] Д-р {FullName} | {specialityFormatted} | Кабінет {RoomNumber} | Розклад: {Schedule}";
    }
}