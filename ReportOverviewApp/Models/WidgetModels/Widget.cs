﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class Widget : IWidget
    {
        public int ID { get; set; }
        [NotMapped]
        public IWidgetOptions Options { get; set; }
        public string Color { get; set; }
        public string Header { get; set; }
        [NotMapped]
        public ISubWidget Body { get; set; }
        public string Footer { get; set; }

    }
}
