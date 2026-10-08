using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private string _roomNumber = "";
    public string FullName => $"{FirstName} {LastName}";

    public int Id { get; }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ім'я не може бути порожнім.", nameof(FirstName));
            if (value.Length > 50)
                throw new ArgumentException("Ім'я не може бути довшим за 50 символів.", nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Прізвище не може бути порожнім.", nameof(LastName));
            if (value.Length > 50)
                throw new ArgumentException("Прізвище не може бути довшим за 50 символів.", nameof(LastName));
            _lastName = value;
        }
    }

    public Speciality Speciality { get; set; }
    public WorkSchedule Schedule { get; set; }

    public string RoomNumber
    {
        get => _roomNumber;
        set => _roomNumber = value;
    }

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor(string firstName, string lastName, Speciality speciality, WorkSchedule schedule, string roomNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        Schedule = schedule;
        RoomNumber = roomNumber;
        Id = _nextId++;
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