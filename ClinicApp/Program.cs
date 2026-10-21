using System.Globalization;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Interfaces;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

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



clinic.Doctors.Add(new Doctor("Денис", "Захарченко", Speciality.Cardiology, morning, "201", "AB123456", "0501112233"));
clinic.Doctors.Add(new Doctor("Ірина", "Гончарук", Speciality.Pediatrics, evening, "202", "AB234567", "0672223344"));
clinic.Doctors.Add(new Doctor("Павло", "Марченко", Speciality.Neurology, morning, "203", "AB345678", "0633334455"));
clinic.Doctors.Add(new Doctor("Олена", "Яковенко", Speciality.Dermatology, evening, "204", "AB456789", "0974445566"));
clinic.Doctors.Add(new Doctor("Святослав", "Білоус", Speciality.General, morning, "205", "AB567890", "0505556677"));

clinic.Appointments.Book(1, 1, DateTime.Today.AddHours(9), 30);
clinic.Appointments.Book(2, 2, DateTime.Today.AddHours(15), 30);
clinic.Appointments.Book(3, 3, DateTime.Today.AddHours(11), 45);






Console.WriteLine("\n=== Медичні записи: тестові дані ===");
clinic.MedicalRecords.Add(new Diagnosis(1, 1, today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
clinic.MedicalRecords.Add(new Diagnosis(1, 1, today.AddDays(-5), "J06.9", "Гострий ринофарингіт"));
clinic.MedicalRecords.Add(new LabResult(1, 1, today.AddDays(-4), "Гемоглобін", 145, "г/л", "120–160", true));
clinic.MedicalRecords.Add(new LabResult(1, 1, today.AddDays(-4), "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
clinic.MedicalRecords.Add(new Prescription(1, 1, today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
clinic.MedicalRecords.Add(new Prescription(2, 2, today.AddDays(-40), "Амоксицилін", "500 мг", 10, "після їжі"));
clinic.MedicalRecords.Add(new Diagnosis(2, 3, today.AddDays(-20), "G43.9", "Мігрень"));
clinic.MedicalRecords.Add(new LabResult(3, 5, today.AddDays(-2), "Глюкоза", 5.1, "ммоль/л", "3.9–5.6", true));

Console.WriteLine("\n=== Поліморфізм: усі записи пацієнта #1 ===");
MedicalRecord[] patientRecords = clinic.MedicalRecords.GetByPatient(1);
for (int i = 0; i < patientRecords.Length; i++)
{
    Console.WriteLine($"{patientRecords[i].GetRecordType()}: {patientRecords[i].GetSummary()}");
}
clinic.MedicalRecords.DisplayPatientSummary(1);

RunMainMenu(clinic);

static void PrintBoxLine(string text)
{
    Console.WriteLine("║" + text.PadRight(46) + "║");
}

static void RunMainMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("╔" + new string('═', 46) + "╗");
        PrintBoxLine("           МЕДИЧНА КЛІНІКА");
        Console.WriteLine("╠" + new string('═', 46) + "╣");
        PrintBoxLine("  1. Пацієнти       — реєстрація, пошук");
        PrintBoxLine("  2. Лікарі         — персонал, розклад");
        PrintBoxLine("  3. Записи         — прийоми, скасування");
        PrintBoxLine("  4. Медична картка — діагнози, рецепти");
        PrintBoxLine("  5. Рахунки        — оплата, борги");
        PrintBoxLine("  6. Звіт           — загальна статистика");
        PrintBoxLine("  0. Вийти");
        Console.WriteLine("╚" + new string('═', 46) + "╝");
        Console.Write("Оберіть розділ: ");

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
                RunMedicalRecordsMenu(clinic);
                break;
            case "5":
                RunBillingMenu(clinic);
                break;
            case "6":
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
                {
                    try
                    {
                        Console.Write("Ім'я: ");
                        string fn = Console.ReadLine() ?? "";
                        Console.Write("Прізвище: ");
                        string ln = Console.ReadLine() ?? "";
                        Console.Write("Телефон (10 цифр): ");
                        string phone = Console.ReadLine() ?? "";
                        Console.WriteLine("Оберіть групу крові (0 - Unknown, 1 - APositive, 2 - ANegative, 3 - BPositive, 4 - BNegative, 5 - ABPositive, 6 - ABNegative, 7 - OPositive, 8 - ONegative): ");
                        Console.Write("Номер: ");
                        int.TryParse(Console.ReadLine(), out int btIndex);
                        if (!Enum.IsDefined(typeof(BloodType), btIndex))
                            throw new ArgumentException("Невідома група крові.", nameof(btIndex));

                        manager.Add(new Patient(fn, ln, DateTime.Today.AddYears(-20), (BloodType)btIndex, phone));
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                    }
                    break;
                }
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
        Console.WriteLine("4. Додати лікаря");
        Console.WriteLine("5. Вільні години лікаря");
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
            case "4":
                {
                    try
                    {
                        Console.Write("Ім'я: ");
                        string fn = Console.ReadLine() ?? "";
                        Console.Write("Прізвище: ");
                        string ln = Console.ReadLine() ?? "";

                        foreach (Speciality s in Enum.GetValues(typeof(Speciality)))
                            Console.WriteLine($"{(int)s} - {s}");
                        Console.Write("Номер спеціальності: ");
                        int.TryParse(Console.ReadLine(), out int specIndex);
                        if (!Enum.IsDefined(typeof(Speciality), specIndex))
                            throw new ArgumentException("Невідома спеціальність.", nameof(specIndex));

                        Console.Write("Початок роботи (година, 0-23): ");
                        int.TryParse(Console.ReadLine(), out int start);
                        Console.Write("Кінець роботи (година, 1-24): ");
                        int.TryParse(Console.ReadLine(), out int end);
                        Console.Write("Кабінет: ");
                        string room = Console.ReadLine() ?? "";
                        Console.Write("Номер ліцензії: ");
                        string license = Console.ReadLine() ?? "";
                        Console.Write("Телефон (10 цифр): ");
                        string docPhone = Console.ReadLine() ?? "";

                        WorkSchedule schedule = new WorkSchedule(start, end);
                        manager.Add(new Doctor(fn, ln, (Speciality)specIndex, schedule, room, license, docPhone));
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                    }
                 
                    break;
                }
          
            case "5":
            {
                if (!AskInt("ID лікаря: ", out int docId))
                    break;

                Doctor? found = manager.FindById(docId);
                if (found == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    break;
                }
                ISchedulable schedulable = found;

                Console.Write("Дата (dd.MM.yyyy): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime slotDate))
                {
                    Console.WriteLine("Помилка: некоректна дата (формат dd.MM.yyyy).");
                    break;
                }

                if (!AskInt("Скільки слотів: ", out int slotCount))
                    break;

                try
                {
                    DateTime[] slots = schedulable.GetAvailableSlots(slotDate, slotCount);
                    for (int i = 0; i < slots.Length; i++)
                    {
                        Console.WriteLine(slots[i].ToString("HH:mm"));
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine($"Помилка: {e.Message}");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Помилка: {e.Message}");
                }
                break;
            }
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
        Console.WriteLine("8. Скасувати всі записи пацієнта");
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
                {
                    try
                    {
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

                        Console.Write("Тривалість (хвилин): ");
                        int.TryParse(Console.ReadLine(), out int duration);

                        appManager.Book(pId, dId, DateTime.Today.AddHours(h), duration);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine($"Помилка: {e.Message}");
                    }
                    break;
                }
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

            case "8":
                {
                    if (!AskInt("ID пацієнта: ", out int cancelPatientId))
                        break;

                    Console.Write("Причина (Enter — без причини): ");
                    string cancelReason = Console.ReadLine() ?? "";

                    int cancelled = AppointmentManager.CancelAll(appManager.GetByPatient(cancelPatientId), cancelReason);
                    Console.WriteLine($"Скасовано записів: {cancelled}");
                    break;
                }
        }
    }
}
    static void RunMedicalRecordsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Медична картка ────────────");
            Console.WriteLine("1. Картка пацієнта");
            Console.WriteLine("2. Усі записи пацієнта");
            Console.WriteLine("3. Додати діагноз");
            Console.WriteLine("4. Додати аналіз");
            Console.WriteLine("5. Додати рецепт");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string? choice = Console.ReadLine();
            if (choice == "0" || string.IsNullOrEmpty(choice)) break;

            switch (choice)
            {
                case "1":
                    {
                        if (!AskInt("ID пацієнта: ", out int pId)) break;
                        if (clinic.Patients.FindById(pId) == null)
                        {
                            Console.WriteLine("Пацієнта не знайдено.");
                            break;
                        }
                        clinic.MedicalRecords.DisplayPatientSummary(pId);
                        break;
                    }
                case "2":
                    {
                        if (!AskInt("ID пацієнта: ", out int pId)) break;
                        if (clinic.Patients.FindById(pId) == null)
                        {
                            Console.WriteLine("Пацієнта не знайдено.");
                            break;
                        }
                        MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(pId);
                        if (records.Length == 0)
                        {
                            Console.WriteLine("Записів немає.");
                            break;
                        }
                        for (int i = 0; i < records.Length; i++)
                            Console.WriteLine($"{records[i].GetRecordType()}: {records[i].GetSummary()}");
                        break;
                    }
                case "3":
                    AddDiagnosisFromMenu(clinic);
                    break;
                case "4":
                    AddLabResultFromMenu(clinic);
                    break;
                case "5":
                    AddPrescriptionFromMenu(clinic);
                    break;
                case "6":
                    {
                        if (!AskInt("ID лікаря: ", out int dId)) break;
                        if (clinic.Doctors.FindById(dId) == null)
                        {
                            Console.WriteLine("Лікаря не знайдено.");
                            break;
                        }
                        clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(dId));
                        break;
                    }
            }
        }
    }

    static bool AskInt(string prompt, out int value)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out value)) return true;
        Console.WriteLine("Некоректне число.");
        return false;
    }

    static bool AskPatientAndDoctor(Clinic clinic, out int pId, out int dId)
    {
        pId = 0; dId = 0;

        Console.WriteLine("\nПацієнти:");
        clinic.Patients.DisplayAll();
        Console.WriteLine("\nЛікарі:");
        clinic.Doctors.DisplayAll();

        if (!AskInt("ID пацієнта: ", out pId)) return false;
        if (!AskInt("ID лікаря: ", out dId)) return false;

        if (clinic.Patients.FindById(pId) == null)
        {
            Console.WriteLine("Пацієнта не знайдено.");
            return false;
        }
        if (clinic.Doctors.FindById(dId) == null)
        {
            Console.WriteLine("Лікаря не знайдено.");
            return false;
        }
        return true;
    }

    static void AddDiagnosisFromMenu(Clinic clinic)
    {
        if (!AskPatientAndDoctor(clinic, out int pId, out int dId)) return;

        Console.Write("Код діагнозу (напр. J06.9): ");
        string code = Console.ReadLine() ?? "";
        Console.Write("Опис: ");
        string desc = Console.ReadLine() ?? "";
        if (!AskInt("Хронічне? (1=так, 0=ні): ", out int chronic)) return;

        try
        {
            Diagnosis d = new Diagnosis(pId, dId, DateTime.Today, code, desc, chronic == 1);
            clinic.MedicalRecords.Add(d);

        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }

    static void AddLabResultFromMenu(Clinic clinic)
    {
        if (!AskPatientAndDoctor(clinic, out int pId, out int dId)) return;

        Console.Write("Назва аналізу: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Значення (число): ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Float,
                             CultureInfo.InvariantCulture, out double value))
        {
            Console.WriteLine("Некоректне число (використовуйте крапку, напр. 6.2).");
            return;
        }

        Console.Write("Одиниці виміру: ");
        string units = Console.ReadLine() ?? "";
        Console.Write("Норма (напр. 4.0–9.0): ");
        string norm = Console.ReadLine() ?? "";
        if (!AskInt("В нормі? (1=так, 0=ні): ", out int inNorm)) return;

        try
        {
            LabResult r = new LabResult(pId, dId, DateTime.Today, name, value, units, norm, inNorm == 1);
            clinic.MedicalRecords.Add(r);

        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }

    static void AddPrescriptionFromMenu(Clinic clinic)
    {
        if (!AskPatientAndDoctor(clinic, out int pId, out int dId)) return;

        Console.Write("Препарат: ");
        string drug = Console.ReadLine() ?? "";
        Console.Write("Дозування (10 мг): ");
        string dosage = Console.ReadLine() ?? "";
        if (!AskInt("Кількість днів: ", out int days)) return;
        Console.Write("Інструкція (Enter — пропустити): ");
        string instruction = Console.ReadLine() ?? "";

        try
        {
            Prescription p = string.IsNullOrWhiteSpace(instruction)
                ? new Prescription(pId, dId, DateTime.Today, drug, dosage, days)
                : new Prescription(pId, dId, DateTime.Today, drug, dosage, days, instruction);
            clinic.MedicalRecords.Add(p);

        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка: {e.Message}");
        }
    }
    static void RunBillingMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Рахунки ───────────────────");
            Console.WriteLine("  1. Борги пацієнта");
            Console.WriteLine("  2. Всі неоплачені записи");
            Console.WriteLine("  3. Оплатити запис");
            Console.WriteLine("  4. Загальний борг клініки");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string? choice = Console.ReadLine();
            if (choice == "0" || string.IsNullOrEmpty(choice)) break;

            switch (choice)
            {
                case "1":
                    {
                        if (!AskInt("ID пацієнта: ", out int patientId))
                            break;

                        IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                        Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
                        clinic.Billing.DisplayUnpaid(unpaid);
                        Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
                        break;
                    }
                case "2":
                    {
                        IPayable[] allUnpaid = clinic.Billing.GetAllUnpaid();
                        clinic.Billing.DisplayUnpaid(allUnpaid);
                        break;
                    }
                case "3":
                    {
                        if (!AskInt("ID запису для оплати: ", out int payId))
                            break;

                        if (clinic.Billing.PayAppointment(payId))
                            Console.WriteLine($"Запис [{payId}] оплачено.");
                        else
                            Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                        break;
                    }
                case "4":
                    Console.WriteLine($"Загальний борг по клініці: {clinic.Billing.GetTotalDebt():F2} грн");
                    break;
            }
        }
    }
