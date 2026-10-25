using CardFile.Business.Models;
using CardFile.Business.Services;
using CardFile.Common.Infrastructure;
using CardFile.Wpf.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.Wpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly StudentFileService _service = new StudentFileService();

        public ObservableCollection<StudentViewModel> Students { get; } = new ObservableCollection<StudentViewModel>();

        public StudentViewModel SelectedStudent { get; set; }

        public bool IsEditButtonEnabled => SelectedStudent != null;

        public bool IsDeleteButtonEnabled => SelectedStudent != null;

        public string FileName { get; private set; }

        public string WindowTitle => string.IsNullOrEmpty(FileName) ? "Картотека студентов" : $"Картотека студентов: {Path.GetFileName(FileName)}";

        public MainWindowViewModel()
        {
            MapperInitialization.PreRegister();
        }

        public void WindowLoaded()
        {
            LoadAllData();
        }

        public void Initialized()
        {
            Mapping.Initialize();
        }

        public StudentViewModel GetSelectedStudent()
        {
            return SelectedStudent;
        }

        public StudentViewModel GetNewStudent()
        {
            return new StudentViewModel
            {
                BirthDate = new DateTime(2005, 1, 1),
                Course = 1,
                AverageGrade = 4.0,
                AdmissionDate = new DateTime(DateTime.Today.Year, 9, 1),
            };
        }

        public void SaveEditedStudent(StudentViewModel student)
        {
            var index = Students.ToList().FindIndex(s => s.Id == student.Id);

            if (index < 0)
            {
                throw new Exception("Студент с таким Id не существует");
            }

            var id = _service.Save(ToBusiness(student));

            if (id < 0)
            {
                Students.RemoveAt(index);
            }
            else
            {
                Students[index].LoadViewModel(student);
            }
        }

        public void SaveNewStudent(StudentViewModel student)
        {
            var newStudent = new StudentViewModel();
            newStudent.LoadViewModel(student);

            var id = _service.Save(ToBusiness(student));

            if (id < 0)
            {
                return;
            }

            newStudent.Id = id;
            Students.Add(newStudent);
        }

        public void SelectionChanged()
        {
            OnPropertyChanged(nameof(IsDeleteButtonEnabled));
            OnPropertyChanged(nameof(IsEditButtonEnabled));
        }

        private void LoadAllData()
        {
            var students = _service.GetAll();
            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(ToViewModel(student));
            }
        }

        public void DeleteSelectedStudent()
        {
            if (SelectedStudent == null)
            {
                return;
            }

            _service.Delete(SelectedStudent.Id);
            var index = Students.ToList().FindIndex(s => s.Id == SelectedStudent.Id);

            if (index < 0)
            {
                throw new Exception("Студент с таким Id не существует");
            }

            Students.RemoveAt(index);
            SelectedStudent = null;

            OnPropertyChanged(nameof(SelectedStudent));
        }

        public void SaveToFile(string fileName)
        {
            _service.SaveToFile(fileName);

            FileName = fileName;
            OnPropertyChanged(nameof(WindowTitle));
        }

        public void OpenFromFile(string fileName)
        {
            _service.OpenFromFile(fileName);
            LoadAllData();

            FileName = fileName;
            OnPropertyChanged(nameof(WindowTitle));
        }

        public void SaveToFile()
        {
            try
            {
                _service.SaveToFile(FileName);
            }
            catch (Exception)
            {
                FileName = null;
                throw;
            }
        }

        private StudentViewModel ToViewModel(Student student)
        {
            return Mapping.Mapper.Map<StudentViewModel>(student);
        }

        private Student ToBusiness(StudentViewModel student)
        {
            return Mapping.Mapper.Map<Student>(student);
        }
    }
}
