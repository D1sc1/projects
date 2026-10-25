using CardFile.DataStore.DataCollection;
using CardFile.DataStore.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DataStore.FileDataAccess.FileManagers
{
    public class TextFileManager : IFileManager
    {
        private const int FieldCount = 12;

        public void OpenFromFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fs))
                {
                    var records = new List<StudentDto>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        var lineParts = line.Split(';');
                        if (lineParts.Length != FieldCount)
                        {
                            throw new Exception($"В строке файла {fileName} неверное количество полей");
                        }

                        var newStudent = new StudentDto();

                        if (int.TryParse(lineParts[0], out var id))
                        {
                            newStudent.Id = id;
                        }
                        else
                        {
                            throw new Exception($"В строке файла {fileName} неверное значение Id");
                        }

                        newStudent.LastName = lineParts[1];
                        newStudent.FirstName = lineParts[2];
                        newStudent.MiddleName = lineParts[3];

                        if (DateTime.TryParse(lineParts[4], out var birthDate))
                        {
                            newStudent.BirthDate = birthDate;
                        }
                        else
                        {
                            throw new Exception($"В строке файла {fileName} неверное значение BirthDate");
                        }

                        newStudent.Faculty = lineParts[5];

                        if (int.TryParse(lineParts[6], out var course))
                        {
                            newStudent.Course = course;
                        }
                        else
                        {
                            throw new Exception($"В строке файла {fileName} неверное значение Course");
                        }

                        newStudent.Group = lineParts[7];
                        newStudent.RecordBookNumber = lineParts[8];

                        if (double.TryParse(lineParts[9], out var averageGrade))
                        {
                            newStudent.AverageGrade = averageGrade;
                        }
                        else
                        {
                            throw new Exception($"В строке файла {fileName} неверное значение AverageGrade");
                        }

                        if (DateTime.TryParse(lineParts[10], out var admissionDate))
                        {
                            newStudent.AdmissionDate = admissionDate;
                        }
                        else
                        {
                            throw new Exception($"В строке файла {fileName} неверное значение AdmissionDate");
                        }

                        if (lineParts[11] == "-")
                        {
                            newStudent.DismissalDate = null;
                        }
                        else if (DateTime.TryParse(lineParts[11], out var dismissalDate))
                        {
                            newStudent.DismissalDate = dismissalDate;
                        }
                        else
                        {
                            throw new Exception($"В строке файла {fileName} неверное значение DismissalDate");
                        }

                        records.Add(newStudent);
                    }

                    collection.ReplaceAll(records);
                }
            }
        }

        public void SaveToFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fs))
                {
                    foreach (var item in collection.GetAll())
                    {
                        writer.WriteLine($"{item.Id};{item.LastName};{item.FirstName};{item.MiddleName};" +
                            $"{item.BirthDate.ToShortDateString()};{item.Faculty};{item.Course};{item.Group};" +
                            $"{item.RecordBookNumber};{item.AverageGrade};{item.AdmissionDate.ToShortDateString()};" +
                            $"{item.DismissalDate?.ToShortDateString() ?? "-"}");
                    }
                }
            }
        }
    }
}
