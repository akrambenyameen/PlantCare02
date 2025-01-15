using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Repositries
{
    public interface IEmailService
    {
        Task sendEmailAsync(string to, string subject, string body);
    }
}
