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



    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (value.Date > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(BirthDate), "Дата народження не може бути в майбутньому.");
            if (value.Year < 1900)
                throw new ArgumentOutOfRangeException(nameof(BirthDate), "Дата народження не може бути раніше 1900 року.");
            _birthDate = value;
        }
    }

    public BloodType BloodType { get; set; }


    public string Phone
    {
        get => _phone;
        set
        {
            if (value == null || value.Length != 10)
                throw new ArgumentException("Телефон має містити рівно 10 символів.", nameof(Phone));
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    throw new ArgumentException("Телефон має містити лише цифри.", nameof(Phone));
            }
            _phone = value;
        }
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
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        BloodType = bloodType;
        Phone = phone;
        Id = _nextId++;
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