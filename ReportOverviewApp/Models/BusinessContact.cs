using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class BusinessContact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public string FullName { get => $"{FirstName} {LastName}";}
        
    }
}
