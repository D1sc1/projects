using CardFile.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DataStore.Dtos
{
    public class StudentDto
    {
        // DTO = Data Transfer Object

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Факультет
        /// </summary>
        public string Faculty { get; set; }

        /// <summary>
        /// Курс
        /// </summary>
        public int Course { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Номер зачётной книжки
        /// </summary>
        public string RecordBookNumber { get; set; }

        /// <summary>
        /// Средний балл
        /// </summary>
        public double AverageGrade { get; set; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public DateTime AdmissionDate { get; set; }

        /// <summary>
        /// Дата отчисления (null - обучается)
        /// </summary>
        public DateTime? DismissalDate { get; set; }

        public StudentDto Clone()
        {
            return Mapping.Mapper.Map<StudentDto>(this);
        }
    }
}
