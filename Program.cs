using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Student
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int Course { get; set; }
    public string StudentID { get; set; }
    public string Gender { get; set; }
    public double AverageGrade { get; set; }
    public string RecordBook { get; set; }

    public override string ToString()
    {
        return $"{LastName} {FirstName}, курс: {Course}, середній бал: {AverageGrade}, стать: {Gender}, залікова: {RecordBook}";
    }
}

class Program
{
    static void Main()
    {
        // Файл буде зберігатися поруч із .cs файлами (у корені проекту)
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\students.txt");

        // Якщо файл не існує — створюємо і записуємо прикладові дані
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не знайдено. Створюю новий файл students.txt з тестовими даними...\n");

            string[] exampleData =
            {
                "Іваненко Іван 2 12345 M 95 7654321",
                "Петренко Олена 1 23456 F 89 7654322",
                "Сидоренко Павло 2 34567 M 92 7654323",
                "Мельник Андрій 3 45678 M 85 7654324",
                "Коваленко Максим 2 56789 M 91 7654325"
            };

            File.WriteAllLines(path, exampleData);
        }

        List<Student> students = new List<Student>();

        // Читання з файлу
        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (data.Length == 7)
                {
                    students.Add(new Student
                    {
                        LastName = data[0],
                        FirstName = data[1],
                        Course = int.Parse(data[2]),
                        StudentID = data[3],
                        Gender = data[4],
                        AverageGrade = double.Parse(data[5]),
                        RecordBook = data[6]
                    });
                }
            }
        }

        // Фільтрація: 2 курс, чоловіча стать, середній бал >= 90
        var excellentMaleStudents = students
            .Where(s => s.Course == 2 && s.Gender == "M" && s.AverageGrade >= 90)
            .ToList();

        Console.WriteLine($"Кількість студентів-чоловіків 2-го курсу, які займаються на відмінно: {excellentMaleStudents.Count}\n");

        if (excellentMaleStudents.Count > 0)
        {
            Console.WriteLine("Їхні дані:");
            foreach (var s in excellentMaleStudents)
                Console.WriteLine(s);
        }
        else
        {
            Console.WriteLine("Немає студентів, які відповідають умові.");
        }

        Console.WriteLine($"\nФайл з даними знаходиться за шляхом:\n{Path.GetFullPath(path)}");
    }
}
