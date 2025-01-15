using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }

        // Foreign keys
        public int? PlantId { get; set; }
        public Plant Plant { get; set; }

        public int? DiseaseId { get; set; }
        public Disease Disease { get; set; }
    }

}
