using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PatientManager pm = new PatientManager();
DoctorManager dm = new DoctorManager();
AppointmentManager am = new AppointmentManager();

DateTime today = DateTime.Today;

pm.Add(new Patient("Тарас", "Ковальчук", today.AddYears(-41), BloodType.APositive, "0501234567"));
pm.Add(new Patient("Софія", "Мельник", today.AddYears(-33), BloodType.BNegative, "0672345678"));

WorkSchedule morning = new WorkSchedule(8, 16);
WorkSchedule evening = new WorkSchedule(14, 22);

Doctor doc1 = new Doctor("Олександр", "Франко", Speciality.Cardiology, morning, "101");
Doctor doc2 = new Doctor("Марія", "Лисенко", Speciality.Pediatrics, evening, "102");
dm.Add(doc1);
dm.Add(doc2);

Console.WriteLine("\n=== Перевантаження FindBySpeciality ===");
Doctor[] cardiologistsEnum = dm.FindBySpeciality(Speciality.Cardiology);
Doctor[] cardiologistsString = dm.FindBySpeciality("кардіо");

Console.WriteLine($"За enum (Cardiology): {cardiologistsEnum.Length}");
Console.WriteLine($"За рядком ('кардіо'): {cardiologistsString.Length}");

Console.WriteLine("\n=== Перевантаження GetByDate ===");
am.Add(new Appointment(pm[0]!, doc1, new DateTime(2026, 5, 10, 10, 0, 0)));
Appointment[] appts1 = am.GetByDate(new DateTime(2026, 5, 10));
Appointment[] appts2 = am.GetByDate(2026, 5, 10);

Console.WriteLine($"За DateTime: {appts1.Length}");
Console.WriteLine($"За (2026, 5, 10): {appts2.Length}");

Console.WriteLine("\n=== TryFindById та out ===");
if (pm.TryFindById(1, out Patient? patient))
{
    Console.WriteLine($"Знайдено: {patient?.FullName}");
}
else
{
    Console.WriteLine("Пацієнта не знайдено.");
}

Console.WriteLine("\n=== FindByBloodType ===");
Patient[] aPositives = pm.FindByBloodType(BloodType.APositive);
Console.WriteLine($"Пацієнтів з A+: {aPositives.Length}");

Console.WriteLine("\n=== Оператори ?. та ?? ===");
string name1 = pm.FindById(1)?.FullName ?? "не знайдено";
string name2 = pm.FindById(99)?.FullName ?? "не знайдено";

Console.WriteLine($"ID 1: {name1}");
Console.WriteLine($"ID 99: {name2}");