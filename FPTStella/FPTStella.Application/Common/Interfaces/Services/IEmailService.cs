using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsyncMailJet(string to, string subject, string htmlBody);
    }
}
