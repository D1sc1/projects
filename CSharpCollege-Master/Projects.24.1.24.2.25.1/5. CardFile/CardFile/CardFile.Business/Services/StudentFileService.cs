using CardFile.Business.Infrastructure;
using CardFile.Business.Models;
using CardFile.Common.Infrastructure;
using CardFile.DataStore.DataCollection;
using CardFile.DataStore.Dtos;
using CardFile.DataStore.FileDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.Business.Services
{
    public class StudentFileService
    {
        private readonly StudentCollection _collection = new StudentCollection();

        public StudentFileService()
        {
            MapperInitialization.PreRegister();
        }

        public IEnumerable<Student> GetAll()
        {
            var students = _collection.GetAll();
            return students.Select(FromDto).ToList();
        }

        public int Save(Student student)
        {
            return _collection.Save(ToDto(student));
        }

        public bool Delete(int studentId)
        {
            return _collection.Delete(studentId);
        }

        public void SaveToFile(string fileName)
        {
            var fileDataService = new FileDataService();
            fileDataService.SaveToFile(fileName, _collection);
        }

        public void OpenFromFile(string fileName)
        {
            var fileDataService = new FileDataService();
            fileDataService.OpenFromFile(fileName, _collection);
        }

        private Student FromDto(StudentDto student)
        {
            return Mapping.Mapper.Map<Student>(student);
        }

        private StudentDto ToDto(Student student)
        {
            return Mapping.Mapper.Map<StudentDto>(student);
        }
    }
}
