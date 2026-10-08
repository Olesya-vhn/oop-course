namespace ClinicApp;

public class Patient
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public BloodType BloodType { get; set; }
    public string Phone { get; set; }

    public string FullName => $"{FirstName} {LastName}";

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