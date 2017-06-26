using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class WidgetBuilder : IBuilder
    {
        Widget Product;
        private static int ProductID;
        public WidgetBuilder() { }
        private void GiveID() {
            ProductID++;
            Product.ID = ProductID;
        }
        private void GiveID(int ID)
        {
            Product.ID = ID;
        }
        public IBuilder BuildProduct()
        {
            Product = new Widget();
            return this;
        }
        public object ReleaseProduct()
        {
            return Product;
        }
        public IBuilder BuildColor(string color)
        {
            Product.Color = color;
            return this;
        }
        public IBuilder BuildOptions(WidgetOptions options)
        {
            Product.Options.Options.AddRange(options.Options);
            return this;
        }
        public IBuilder BuildOption(string option)
        {
            Product.Options.Options.Add(option);
            return this;
        }
        public IBuilder BuildHeader(string header)
        {
            Product.Header = header;
            return this;
        }
        public IBuilder BuildFooter(string footer)
        {
            Product.Footer = footer;
            return this;
        }
        public IBuilder BuildSubWidget(ISubWidget subwidget)
        {
            Product.Body = subwidget;
            return this;
        }
    }
}
