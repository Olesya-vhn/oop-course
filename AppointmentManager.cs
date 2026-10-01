namespace ClinicApp;

public class AppointmentManager
{
    private Appointment[] _appointments;
    private int _count;

    public int Count => _count;

    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null;
            return _appointments[index];
        }
    }

    public AppointmentManager(int capacity = 10)
    {
        _appointments = new Appointment[capacity];
        _count = 0;
    }

    public void Add(Appointment appointment)
    {
        if (_count == _appointments.Length)
        {
            Array.Resize(ref _appointments, _appointments.Length * 2);
        }
        _appointments[_count++] = appointment;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DateTime.Date == date.Date)
            {
                matchCount++;
            }
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DateTime.Date == date.Date)
            {
                result[index++] = _appointments[i];
            }
        }

        return result;
    }

    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public bool Remove(int id)
    {
        int index = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                index = i;
                break;
            }
        }

        if (index == -1) return false;

        for (int i = index; i < _count - 1; i++)
        {
            _appointments[i] = _appointments[i + 1];
        }

        _appointments[--_count] = null!;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список записів порожній.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_appointments[i]);
        }
    }
}