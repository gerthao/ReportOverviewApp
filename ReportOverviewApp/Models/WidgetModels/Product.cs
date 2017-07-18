using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class Product<T>
    {
        private List<T> parts;
        private T Item;
        public void Add(T part)
        {
            parts.Add(part);
        }
        public void Discard()
        {
            parts.Clear();
        }
    }
}
