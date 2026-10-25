using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DataStore.FileDataAccess.Entities
{
    public class JsonStudentCollection
    {
        public int NextId { get; set; }

        public List<JsonStudent> Students { get; set; } = new List<JsonStudent>();
    }
}
