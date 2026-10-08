using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private DateTime _birthDate;
    private string _phone = "";
    public string FullName => $"{FirstName} {LastName}";

    public int Id { get; }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public BloodType BloodType { get; set; }

    public string Phone
    {
        get => _phone;
        set => _phone = value;
    }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }

    public Patient(string firstName, string lastName, DateTime birthDate, BloodType bloodType, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        BloodType = bloodType;
        Phone = phone;
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today, BloodType.Unknown, "0000000000")
    {
    }

    public override string ToString()
    {
        string ageFormatted = ClinicFormatter.FormatAge(Age);
        string bloodFormatted = ClinicFormatter.FormatBloodType(BloodType);
        string phoneFormatted = ClinicFormatter.FormatPhone(Phone);

        return $"[{Id}] {FullName} ({ageFormatted}) | Група крові: {bloodFormatted} | Тел: {phoneFormatted}";
    }
}