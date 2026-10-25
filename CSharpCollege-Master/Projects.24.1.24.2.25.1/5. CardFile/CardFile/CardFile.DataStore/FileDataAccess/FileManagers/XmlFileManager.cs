using CardFile.Common.Infrastructure;
using CardFile.DataStore.DataCollection;
using CardFile.DataStore.Dtos;
using CardFile.DataStore.FileDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CardFile.DataStore.FileDataAccess.FileManagers
{
    internal class XmlFileManager : IFileManager
    {
        public void OpenFromFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fs))
                {
                    var serializer = new XmlSerializer(typeof(XmlStudentCollection));
                    var xmlCollection = (XmlStudentCollection)serializer.Deserialize(reader);

                    collection.ReplaceAll(Mapping.Mapper.Map<List<StudentDto>>(xmlCollection.Students), xmlCollection.NextId);
                }
            }
        }

        public void SaveToFile(string fileName, StudentCollection collection)
        {
            var xmlCollection = new XmlStudentCollection 
            { 
                NextId = collection.NextId,
                Students = Mapping.Mapper.Map<List<XmlStudent>>(collection.GetAll()),
            };

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fs))
                {
                    var serializer = new XmlSerializer(typeof(XmlStudentCollection));
                    serializer.Serialize(writer, xmlCollection);
                }
            }
        }
    }
}
