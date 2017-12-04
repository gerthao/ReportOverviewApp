using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Services
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string Content { get; set; }

        public ICollection<EmailAddress> ToAddresses { get; set; }
        public ICollection<EmailAddress> FromAddresses { get; set; }

        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }
    }
}
