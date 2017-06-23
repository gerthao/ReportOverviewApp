using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class Widget : IWidget
    {
        public int ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public WidgetOptions Options { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Header { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISubWidget Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Footer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
