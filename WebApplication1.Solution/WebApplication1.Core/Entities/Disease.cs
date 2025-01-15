using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Disease : BaseEntity
    {
        public string Name { get; set; }

        // Foreign keys
        public int PlantId { get; set; }
        public Plant Plant { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        // Navigation properties
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
