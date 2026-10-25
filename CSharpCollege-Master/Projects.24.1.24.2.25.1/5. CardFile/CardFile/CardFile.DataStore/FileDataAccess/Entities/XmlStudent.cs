using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CardFile.DataStore.FileDataAccess.Entities
{
    public class XmlStudent
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("LastName")]
        public string LastName { get; set; }

        [XmlAttribute("FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute("MiddleName")]
        public string MiddleName { get; set; }

        [XmlIgnore]
        public DateTime BirthDate { get; set; }

        [XmlAttribute("BirthDate")]
        public string BirthDateText
        {
            get => BirthDate.ToShortDateString();
            set => BirthDate = DateTime.Parse(value);
        }

        [XmlAttribute("Faculty")]
        public string Faculty { get; set; }

        [XmlAttribute("Course")]
        public int Course { get; set; }

        [XmlAttribute("Group")]
        public string Group { get; set; }

        [XmlAttribute("RecordBookNumber")]
        public string RecordBookNumber { get; set; }

        [XmlAttribute("AverageGrade")]
        public double AverageGrade { get; set; }

        [XmlIgnore]
        public DateTime AdmissionDate { get; set; }

        [XmlAttribute("AdmissionDate")]
        public string AdmissionDateText
        {
            get => AdmissionDate.ToShortDateString();
            set => AdmissionDate = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime? DismissalDate { get; set; }

        [XmlAttribute("DismissalDate")]
        public string DismissalDateText
        {
            get => DismissalDate?.ToShortDateString() ?? "-";
            set => DismissalDate = (value == "-") ? (DateTime?)null : DateTime.Parse(value);
        }
    }
}
