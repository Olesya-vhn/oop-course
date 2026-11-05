
using ClinicApp.Enums;
using ClinicApp.Models;
namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count => _patients.Count;

    public Patient this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
                return null!;
            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, out Patient patient)
    {
        patient = FindById(id)!;
        return patient != null;
    }

    public Patient[] FindByName(string query)
    {
        string q = (query ?? "").Trim().ToLower();
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(q) || _patients[i].LastName.ToLower().Contains(q))
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }

    public void DisplayStats()
    {
        Console.WriteLine("=== Статистика пацієнтів ===");
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        int totalAge = 0;
        int minIdx = 0;
        int maxIdx = 0;
        int adultsCount = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            int age = _patients[i].Age;
            totalAge += age;

            if (age < _patients[minIdx].Age)
            {
                minIdx = i;
            }

            if (age > _patients[maxIdx].Age)
            {
                maxIdx = i;
            }

            if (age >= 18)
            {
                adultsCount++;
            }
        }

        double avgAge = (double)totalAge / _patients.Count;

        Console.WriteLine($"Усього: {_patients.Count}");
        Console.WriteLine($"Середній вік: {avgAge:F1} р.");
        Console.WriteLine($"Наймолодший: {_patients[minIdx].FullName} ({_patients[minIdx].Age} р.)");
        Console.WriteLine($"Найстарший: {_patients[maxIdx].FullName} ({_patients[maxIdx].Age} р.)");
        Console.WriteLine($"Дорослі: {adultsCount} з {_patients.Count}");
    }
}