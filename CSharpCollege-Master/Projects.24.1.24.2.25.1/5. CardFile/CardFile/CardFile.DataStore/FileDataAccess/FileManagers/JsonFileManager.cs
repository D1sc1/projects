using CardFile.Common.Infrastructure;
using CardFile.DataStore.DataCollection;
using CardFile.DataStore.Dtos;
using CardFile.DataStore.FileDataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CardFile.DataStore.FileDataAccess.FileManagers
{
    internal class JsonFileManager : IFileManager
    {
        public void OpenFromFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fs))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        var serializer = new JsonSerializer();
                        var jsonCollection = serializer.Deserialize<JsonStudentCollection>(jsonReader);

                        collection.ReplaceAll(Mapping.Mapper.Map<List<StudentDto>>(jsonCollection.Students), jsonCollection.NextId);
                    }
                }
            }
        }

        public void SaveToFile(string fileName, StudentCollection collection)
        {
            var jsonCollection = new JsonStudentCollection
            {
                NextId = collection.NextId,
                Students = Mapping.Mapper.Map<List<JsonStudent>>(collection.GetAll()),
            };
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fs))
                {
                    var serializer = new JsonSerializer { Formatting = Formatting.Indented, };
                    serializer.Serialize(writer, jsonCollection);
                }
            }
        }
    }
}
