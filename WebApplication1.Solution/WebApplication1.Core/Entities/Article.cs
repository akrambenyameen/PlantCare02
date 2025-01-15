using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Article : BaseEntity
    {

        public string Symptoms { get; set; }
        public string DiseaseName { get; set; }
        public string Cause { get; set; }

        // Navigation properties
        public Disease Disease { get; set; }

    }
}
