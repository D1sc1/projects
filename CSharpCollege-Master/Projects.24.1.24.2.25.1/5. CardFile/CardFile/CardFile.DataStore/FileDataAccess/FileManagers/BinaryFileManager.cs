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
    internal class BinaryFileManager : IFileManager
    {
        public void OpenFromFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fs))
                {
                    var records = new List<StudentDto>();

                    try
                    {
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            var newStudent = new StudentDto();

                            newStudent.Id = reader.ReadInt32();
                            newStudent.LastName = reader.ReadString();
                            newStudent.FirstName = reader.ReadString();
                            newStudent.MiddleName = reader.ReadString();
                            newStudent.BirthDate = new DateTime(reader.ReadInt64());
                            newStudent.Faculty = reader.ReadString();
                            newStudent.Course = reader.ReadInt32();
                            newStudent.Group = reader.ReadString();
                            newStudent.RecordBookNumber = reader.ReadString();
                            newStudent.AverageGrade = reader.ReadDouble();
                            newStudent.AdmissionDate = new DateTime(reader.ReadInt64());

                            var isDismissalDatePresent = reader.ReadBoolean();
                            if (isDismissalDatePresent)
                            {
                                newStudent.DismissalDate = new DateTime(reader.ReadInt64());
                            }
                            else
                            {
                                newStudent.DismissalDate = null;
                            }

                            records.Add(newStudent);
                        }
                    }
                    catch
                    {
                        throw new Exception($"Неверные данные в файле {fileName}");
                    }

                    collection.ReplaceAll(records);
                }
            }
        }

        public void SaveToFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fs))
                {
                    foreach (var item in collection.GetAll())
                    {
                        writer.Write(item.Id);
                        writer.Write(item.LastName ?? string.Empty);
                        writer.Write(item.FirstName ?? string.Empty);
                        writer.Write(item.MiddleName ?? string.Empty);
                        writer.Write(item.BirthDate.Ticks);
                        writer.Write(item.Faculty ?? string.Empty);
                        writer.Write(item.Course);
                        writer.Write(item.Group ?? string.Empty);
                        writer.Write(item.RecordBookNumber ?? string.Empty);
                        writer.Write(item.AverageGrade);
                        writer.Write(item.AdmissionDate.Ticks);

                        writer.Write(item.DismissalDate.HasValue);
                        if (item.DismissalDate.HasValue)
                        {
                            writer.Write(item.DismissalDate.Value.Ticks);
                        }
                    }
                }
            }
        }
    }
}
