using CardFile.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.Wpf.ViewModels
{
    public class StudentViewModel : ViewModelBase
    {
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

        public string Fio => $"{LastName} {FirstName} {MiddleName}".Trim();

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }

        public string BirthDateText => BirthDate.ToShortDateString();

        /// <summary>
        /// Факультет
        /// </summary>
        public string Faculty { get; set; }

        /// <summary>
        /// Курс
        /// </summary>
        public int Course { get; set; }

        /// <summary>
        /// Курсы для выбора в окне правки
        /// </summary>
        public IEnumerable<int> Courses => Enumerable.Range(1, 6);

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

        public string AverageGradeText => AverageGrade.ToString("F2");

        /// <summary>
        /// Дата поступления
        /// </summary>
        public DateTime AdmissionDate { get; set; }

        public string AdmissionDateText => AdmissionDate.ToShortDateString();

        /// <summary>
        /// Дата отчисления
        /// </summary>
        public DateTime? DismissalDate { get; set; }

        public string DismissalDateText => DismissalDate?.ToShortDateString() ?? "Обучается";

        public bool IsStudyingNow { get; set; }

        public bool IsDismissalDateEnabled => !IsStudyingNow;

        public void IsStudyingNowChecked()
        {
            DismissalDate = null;

            OnPropertyChanged(nameof(DismissalDate));
            OnPropertyChanged(nameof(IsDismissalDateEnabled));
        }

        public void IsStudyingNowUnchecked()
        {
            DismissalDate = DateTime.Today;

            OnPropertyChanged(nameof(DismissalDate));
            OnPropertyChanged(nameof(IsDismissalDateEnabled));
        }

        public void LoadViewModel(StudentViewModel model)
        {
            Mapping.Mapper.Map(model, this);

            IsStudyingNow = !model.DismissalDate.HasValue;

            UpdateAll();
        }

        private void UpdateAll()
        {
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(MiddleName));
            OnPropertyChanged(nameof(Fio));
            OnPropertyChanged(nameof(BirthDate));
            OnPropertyChanged(nameof(BirthDateText));
            OnPropertyChanged(nameof(Faculty));
            OnPropertyChanged(nameof(Course));
            OnPropertyChanged(nameof(Group));
            OnPropertyChanged(nameof(RecordBookNumber));
            OnPropertyChanged(nameof(AverageGrade));
            OnPropertyChanged(nameof(AverageGradeText));
            OnPropertyChanged(nameof(AdmissionDate));
            OnPropertyChanged(nameof(AdmissionDateText));
            OnPropertyChanged(nameof(DismissalDate));
            OnPropertyChanged(nameof(DismissalDateText));
            OnPropertyChanged(nameof(IsStudyingNow));
            OnPropertyChanged(nameof(IsDismissalDateEnabled));
        }
    }
}
