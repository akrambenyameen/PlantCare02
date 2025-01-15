using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Plant : BaseEntity
    {
        public string plantName { get; set; }

        public double tempreture { get; set; }
        public double humidity { get; set; }
        public double soilHumidity { get; set; }

        public string lightIntensity { get; set; }

        public Data Data { get; set; }

        // Navigation properties
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<Disease> Diseases { get; set; } = new List<Disease>();

    }
}
