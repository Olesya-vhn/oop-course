namespace ClinicApp;

public class Patient
{
    private static int _nextId = 1;
    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public BloodType BloodType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public string FullName
    {
        get
        {
            return FirstName + " " + LastName;
        }
    }

    public int Age
    {
        get
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    public bool IsAdult
    {
        get
        {
            return Age >= 18;
        }
    }

    public Patient(string firstName, string lastName, DateTime dateOfBirth, BloodType bloodType, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000")
    {
    }

    public Patient()
        : this("Пацієнт", "Невідомий", DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000")
    {
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
        {
            return "Дитина";
        }
        else if (Age < 60)
        {
            return "Дорослий";
        }
        else
        {
            return "Літній";
        }
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Вік: {Age} ({GetAgeCategory()}) | Група: {BloodType} | Тел: {Phone}";
    }
}