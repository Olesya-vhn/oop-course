namespace ClinicApp;

public class DoctorManager
{
    private Doctor[] _doctors;
    private int _count;

    public int Count => _count;

    public Doctor this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null!;
            return _doctors[index];
        }
    }

    public DoctorManager(int capacity = 10)
    {
        _doctors = new Doctor[capacity];
        _count = 0;
    }

    public void Add(Doctor doctor)
    {
        if (_count == _doctors.Length)
        {
            Array.Resize(ref _doctors, _doctors.Length * 2);
        }
        _doctors[_count++] = doctor;
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, out Doctor doctor)
    {
        doctor = FindById(id)!;
        return doctor != null;
    }

    public Doctor[] FindBySpeciality(string query)
    {
        string q = (query ?? "").Trim().ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            string formattedSpec = ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower();
            string enumSpec = _doctors[i].Speciality.ToString().ToLower();

            if (formattedSpec.Contains(q) || enumSpec.Contains(q))
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            string formattedSpec = ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower();
            string enumSpec = _doctors[i].Speciality.ToString().ToLower();

            if (formattedSpec.Contains(q) || enumSpec.Contains(q))
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }

    public Doctor[] FindBySpeciality(Speciality speciality)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }

    public bool Remove(int id)
    {
        int index = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                index = i;
                break;
            }
        }

        if (index == -1) return false;

        for (int i = index; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[--_count] = null!;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }
    }
}