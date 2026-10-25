using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DataStore.FileDataAccess.Entities
{
    public class JsonStudent
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [JsonIgnore]
        public DateTime BirthDate { get; set; }

        [JsonProperty("BirthDate")]
        public string BirthDateText
        {
            get => BirthDate.ToShortDateString();
            set => BirthDate = DateTime.Parse(value);
        }

        public string Faculty { get; set; }

        public int Course { get; set; }

        public string Group { get; set; }

        public string RecordBookNumber { get; set; }

        public double AverageGrade { get; set; }

        [JsonIgnore]
        public DateTime AdmissionDate { get; set; }

        [JsonProperty("AdmissionDate")]
        public string AdmissionDateText
        {
            get => AdmissionDate.ToShortDateString();
            set => AdmissionDate = DateTime.Parse(value);
        }

        [JsonIgnore]
        public DateTime? DismissalDate { get; set; }

        [JsonProperty("DismissalDate")]
        public string DismissalDateText
        {
            get => DismissalDate?.ToShortDateString() ?? "-";
            set => DismissalDate = (value == "-") ? (DateTime?)null : DateTime.Parse(value);
        }
    }
}
