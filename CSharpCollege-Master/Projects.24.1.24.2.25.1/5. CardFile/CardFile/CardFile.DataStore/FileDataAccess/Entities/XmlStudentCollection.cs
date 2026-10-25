using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CardFile.DataStore.FileDataAccess.Entities
{
    [XmlRoot("StudentCollection")]
    public class XmlStudentCollection
    {
        [XmlAttribute("NextId")]
        public int NextId { get; set; }

        [XmlArray("Students")]
        [XmlArrayItem("Student")]
        public List<XmlStudent> Students { get; set; } = new List<XmlStudent>();
    }
}
