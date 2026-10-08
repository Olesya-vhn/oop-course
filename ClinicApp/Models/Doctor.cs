using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private string _roomNumber = "";
    private string _licenseNumber = "";
    private string _phone = "";
    public string FullName => $"{FirstName} {LastName}";

    public int Id { get; }

    public string FirstName
    {
        get => _firstName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));
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


    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));
            _licenseNumber = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);
            _phone = value;
        }
    }

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor(string firstName, string lastName, Speciality speciality, WorkSchedule schedule, string roomNumber, string licenseNumber, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        Schedule = schedule;
        RoomNumber = roomNumber;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Id = _nextId++;
    }

    public Doctor(string firstName, string lastName)
        : this(firstName, lastName, Speciality.General, new WorkSchedule(8, 17), "100", "000000", "0000000000")
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