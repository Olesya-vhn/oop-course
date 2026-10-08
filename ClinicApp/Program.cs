using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Clinic clinic = new Clinic("Медичний Центр «Здоров'я»");
DateTime today = DateTime.Today;

clinic.Patients.Add(new Patient("Ярослав", "Кравченко", today.AddYears(-38), BloodType.APositive, "0631234567"));
clinic.Patients.Add(new Patient("Вікторія", "Шевчук", today.AddYears(-29), BloodType.BPositive, "0972345678"));
clinic.Patients.Add(new Patient("Богдан", "Дмитренко", today.AddYears(-45), BloodType.ONegative, "0503456789"));
clinic.Patients.Add(new Patient("Анастасія", "Данилюк", today.AddYears(-22), BloodType.ABPositive, "0684567890"));
clinic.Patients.Add(new Patient("Роман", "Василенко", today.AddYears(-50), BloodType.ANegative, "0935678901"));

WorkSchedule morning = new WorkSchedule(8, 16);
WorkSchedule evening = new WorkSchedule(14, 22);

WorkSchedule copy = morning;
copy = new WorkSchedule(9, 17);

clinic.Doctors.Add(new Doctor("Денис", "Захарченко", Speciality.Cardiology, morning, "201"));
clinic.Doctors.Add(new Doctor("Ірина", "Гончарук", Speciality.Pediatrics, evening, "202"));
clinic.Doctors.Add(new Doctor("Павло", "Марченко", Speciality.Neurology, morning, "203"));
clinic.Doctors.Add(new Doctor("Олена", "Яковенко", Speciality.Dermatology, evening, "204"));
clinic.Doctors.Add(new Doctor("Святослав", "Білоус", Speciality.General, morning, "205"));

clinic.Appointments.Book(1, 1, DateTime.Today.AddHours(9), 30);
clinic.Appointments.Book(2, 2, DateTime.Today.AddHours(15), 30);
clinic.Appointments.Book(3, 3, DateTime.Today.AddHours(11), 45);

Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
Doctor[] foundDocs = clinic.Doctors.FindBySpeciality("кардіо");
Appointment[] appToday = clinic.Appointments.GetByDate(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

if (clinic.Patients.TryFindById(1, out Patient patient))
{
    Console.WriteLine($"Знайдено: {patient.FullName}");
}
else
{
    Console.WriteLine("Пацієнта не знайдено.");
}

string unknownName = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
Console.WriteLine(unknownName);
Console.WriteLine("\n=== Демонстрація Задачі 3: валідація ===");

try { new Patient("", "Петренко"); }
catch (ArgumentException ex) { Console.WriteLine($"Порожнє ім'я: {ex.Message}"); }

try { new Patient("Іван", "Петренко", DateTime.Today.AddDays(1), BloodType.Unknown, "0501234567"); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Дата в майбутньому: {ex.Message}"); }

try { new Patient("Іван", "Петренко", DateTime.Today.AddYears(-20), BloodType.Unknown, "05012"); }
catch (ArgumentException ex) { Console.WriteLine($"Короткий телефон: {ex.Message}"); }

try { new Appointment(1, 1, DateTime.Today.AddHours(10), 0); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Тривалість 0: {ex.Message}"); }

try { new WorkSchedule(25, 3); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Графік 25-3: {ex.Message}"); }

try { new WorkSchedule(20, 6); }
catch (ArgumentException ex) { Console.WriteLine($"Графік 20-6: {ex.Message}"); }

try { new Patient("", "А"); } catch (ArgumentException) { }
try { new Patient("", "Б"); } catch (ArgumentException) { }
Patient okPatient = new Patient("Марія", "Коваль");
Console.WriteLine($"Id після двох невдалих спроб: {okPatient.Id} (очікується 6)");
RunMainMenu(clinic);

static void RunMainMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n=== Головне меню Клініки ===");
        Console.WriteLine("1. Пацієнти");
        Console.WriteLine("2. Лікарі");
        Console.WriteLine("3. Записи на прийом");
        Console.WriteLine("4. Розклад на сьогодні");
        Console.WriteLine("5. Загальний звіт клініки");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                RunPatientMenu(clinic.Patients);
                break;
            case "2":
                RunDoctorMenu(clinic.Doctors);
                break;
            case "3":
                RunAppointmentMenu(clinic.Appointments, clinic.Patients, clinic.Doctors);
                break;
            case "4":
                clinic.DisplaySchedule(DateTime.Today);
                break;
            case "5":
                clinic.GenerateReport();
                break;
        }
    }
}

static void RunPatientMenu(PatientManager manager)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Пацієнти» ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати пацієнта");
        Console.WriteLine("3. Пошук за ім'ям");
        Console.WriteLine("4. Видалити за ID");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                manager.DisplayAll();
                break;
            case "2":
                Console.Write("Ім'я: ");
                string fn = Console.ReadLine() ?? "";
                Console.Write("Прізвище: ");
                string ln = Console.ReadLine() ?? "";
                Console.WriteLine("Оберіть групу крові (0 - Unknown, 1 - APositive, 2 - ANegative, 3 - BPositive, 4 - BNegative, 5 - ABPositive, 6 - ABNegative, 7 - OPositive, 8 - ONegative): ");
                Console.Write("Номер: ");
                int.TryParse(Console.ReadLine(), out int btIndex);
                BloodType bt = (BloodType)btIndex;
                manager.Add(new Patient(fn, ln, DateTime.Today.AddYears(-20), bt, "0000000000"));
                break;
            case "3":
                Console.Write("Пошуковий запит: ");
                string q = Console.ReadLine() ?? "";
                var res = manager.FindByName(q);
                if (res.Length == 0) Console.WriteLine("Нікого не знайдено.");
                else foreach (var p in res) Console.WriteLine(p);
                break;
            case "4":
                Console.Write("ID для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    if (manager.Remove(id)) Console.WriteLine("Видалено.");
                    else Console.WriteLine("Не знайдено.");
                }
                break;
            case "5":
                manager.DisplayStats();
                break;
        }
    }
}

static void RunDoctorMenu(DoctorManager manager)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Лікарі» ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Пошук за спеціальністю");
        Console.WriteLine("3. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                manager.DisplayAll();
                break;
            case "2":
                Console.Write("Спеціальність для пошуку: ");
                string spec = Console.ReadLine() ?? "";
                var docs = manager.FindBySpeciality(spec);
                if (docs.Length == 0) Console.WriteLine("Лікарів не знайдено.");
                else foreach (var d in docs) Console.WriteLine(d);
                break;
            case "3":
                manager.DisplayStats();
                break;
        }
    }
}

static void RunAppointmentMenu(AppointmentManager appManager, PatientManager pManager, DoctorManager dManager)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Записи» ---");
        Console.WriteLine("1. Майбутні записи");
        Console.WriteLine("2. Записи на сьогодні");
        Console.WriteLine("3. Створити запис");
        Console.WriteLine("4. Скасувати запис");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                appManager.DisplayList(appManager.GetUpcoming());
                break;
            case "2":
                appManager.DisplayList(appManager.GetByDate(DateTime.Today));
                break;
            case "3":
                Console.WriteLine("\nСписок пацієнтів:");
                pManager.DisplayAll();
                Console.Write("ID пацієнта: ");
                int.TryParse(Console.ReadLine(), out int pId);

                Console.WriteLine("\nСписок лікарів:");
                dManager.DisplayAll();
                Console.Write("ID лікаря: ");
                int.TryParse(Console.ReadLine(), out int dId);

                Console.Write("Година прийому сьогодні (0-23): ");
                int.TryParse(Console.ReadLine(), out int h);

                appManager.Book(pId, dId, DateTime.Today.AddHours(h), 30);
                break;
            case "4":
                Console.Write("ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int appId))
                {
                    Console.Write("Причина: ");
                    string reason = Console.ReadLine() ?? "";
                    if (appManager.Cancel(appId, reason)) Console.WriteLine("Запис успішно скасовано.");
                    else Console.WriteLine("Не вдалося скасувати запис.");
                }
                break;
        }
    }
}