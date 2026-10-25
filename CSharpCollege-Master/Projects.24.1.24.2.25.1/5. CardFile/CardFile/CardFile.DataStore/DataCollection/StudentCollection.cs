using CardFile.DataStore.Dtos;
using CardFile.DataStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DataStore.DataCollection
{
    public class StudentCollection
    {
        private readonly List<StudentDto> _students = new List<StudentDto>();

        internal int NextId { get; set; } = 4;

        public StudentCollection()
        {
            _students.Add(new StudentDto
            {
                Id = 1,
                LastName = "Иванов",
                FirstName = "Иван",
                MiddleName = "Иванович",
                BirthDate = new DateTime(2000, 5, 15),
                Faculty = "Информационных технологий",
                Course = 3,
                Group = "ИТ-31",
                RecordBookNumber = "123456",
                AverageGrade = 4.5,
                AdmissionDate = new DateTime(2021, 9, 1),
                DismissalDate = null,
            });
            _students.Add(new StudentDto
            {
                Id = 2,
                LastName = "Петрова",
                FirstName = "Екатерина",
                MiddleName = "Алексеевна",
                BirthDate = new DateTime(2001, 8, 23),
                Faculty = "Экономический",
                Course = 4,
                Group = "ЭК-42",
                RecordBookNumber = "234567",
                AverageGrade = 4.8,
                AdmissionDate = new DateTime(2020, 9, 1),
                DismissalDate = null,
            });
            _students.Add(new StudentDto
            {
                Id = 3,
                LastName = "Сидоров",
                FirstName = "Алексей",
                MiddleName = "Дмитриевич",
                BirthDate = new DateTime(1999, 12, 10),
                Faculty = "Физико-математический",
                Course = 2,
                Group = "ФМ-22",
                RecordBookNumber = "345678",
                AverageGrade = 3.2,
                AdmissionDate = new DateTime(2022, 9, 1),
                DismissalDate = new DateTime(2023, 6, 30),
            });

            MapperInitialization.PreRegister();
        }

        public IEnumerable<StudentDto> GetAll()
        {
            return _students.Select(s => s.Clone()).ToList();
        }

        public int Save(StudentDto student)
        {
            if (student.Id == 0)
            {
                var newStudent = student.Clone();
                var id = NextId++;
                newStudent.Id = id;
                _students.Add(newStudent);
                return id;
            }

            var index = _students.FindIndex(s => s.Id == student.Id);
            if (index < 0)
            {
                return -1;
            }

            _students[index] = student.Clone();
            return student.Id;
        }

        public bool Delete(int studentId)
        {
            var index = _students.FindIndex(s => s.Id == studentId);
            if (index < 0)
            {
                return false;
            }

            _students.RemoveAt(index);
            return true;
        }

        internal void ReplaceAll(IEnumerable<StudentDto> newCollection)
        {
            _students.Clear();
            _students.AddRange(newCollection);
            NextId = _students.Count > 0 ? _students.Max(s => s.Id) + 1 : 1;
        }

        internal void ReplaceAll(IEnumerable<StudentDto> newCollection, int nextId)
        {
            _students.Clear();
            _students.AddRange(newCollection);
            NextId = nextId;
        }
    }
}
