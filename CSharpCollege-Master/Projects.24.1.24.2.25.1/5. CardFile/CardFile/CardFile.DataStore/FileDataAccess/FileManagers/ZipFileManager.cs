using CardFile.Common.Infrastructure;
using CardFile.DataStore.DataCollection;
using CardFile.DataStore.Dtos;
using CardFile.DataStore.FileDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CardFile.DataStore.FileDataAccess.FileManagers
{
    internal class ZipFileManager : IFileManager
    {
        public void OpenFromFile(string fileName, StudentCollection collection)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
                {
                    try
                    {
                        var entry = archive.GetEntry(Path.GetFileNameWithoutExtension(fileName) + ".xml");
                        using (var es = entry.Open())
                        {
                            using (var reader = new StreamReader(es))
                            {
                                var serializer = new XmlSerializer(typeof(XmlStudentCollection));
                                var xmlCollection = (XmlStudentCollection)serializer.Deserialize(reader);

                                collection.ReplaceAll(Mapping.Mapper.Map<List<StudentDto>>(xmlCollection.Students), xmlCollection.NextId);
                            }
                        }
                    }
                    catch
                    {
                        throw new Exception("Неверный формат файла");
                    }
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
                using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    var entry = archive.CreateEntry(Path.GetFileNameWithoutExtension(fileName) + ".xml");
                    using (var es = entry.Open())
                    {
                        using (var writer = new StreamWriter(es))
                        {
                            var serializer = new XmlSerializer(typeof(XmlStudentCollection));
                            serializer.Serialize(writer, xmlCollection);
                        }
                    }
                }
            }
        }
    }
}
