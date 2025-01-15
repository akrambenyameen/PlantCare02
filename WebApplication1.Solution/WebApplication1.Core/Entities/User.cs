using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Core.Entities
{
    public class User : IdentityUser<int> 
    {
      
        public string name { get; set; }

        [EmailAddress]
       
        public int? activateCode { get; set; }

        public bool? isConfirmed { get; set; }

        public int? forgetCode { get; set; }


        public ICollection<Token> tokens { get; set; }
    }
}
