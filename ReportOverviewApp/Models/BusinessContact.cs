using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class BusinessContact
    {
        public int Id { get; set; }
        [StringLength(64)]
        public string Name { get; set; }

        //public int BusinessOwnerId { get; set; }
        [StringLength(64)]
        public string BusinessOwner { get; set; }
        
        public ICollection<Report> Reports { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
