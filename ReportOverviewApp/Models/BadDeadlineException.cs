using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class BadDeadlineException : Exception
    {
        public BadDeadlineException()
        {
        }

        public BadDeadlineException(string message) : base(message)
        {
        }

        public BadDeadlineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadDeadlineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
