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
    }
}
