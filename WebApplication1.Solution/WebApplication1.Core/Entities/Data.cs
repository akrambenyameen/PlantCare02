using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Data : BaseEntity
    {
        public string details { get; set; }
        public Plant Plant { get; set; }

        public int PlantId { get; set; }
    }
}
