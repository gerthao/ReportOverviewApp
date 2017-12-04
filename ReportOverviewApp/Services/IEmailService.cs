using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
        Task<List<EmailMessage>> ReceiveEmailAsync(int maxCount = 10);
    }
}
