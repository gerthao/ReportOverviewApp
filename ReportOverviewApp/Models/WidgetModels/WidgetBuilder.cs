﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class WidgetBuilder
    {
        Widget Product;
        private static int ProductID;
        public WidgetBuilder() { }
        //private void GiveID() {
        //    ProductID++;
        //    Product.ID = ProductID;
        //}
        public WidgetBuilder BuildID(int ID)
        {
            Product.ID = ID;
            return this;
        }
        public WidgetBuilder BuildProduct()
        {
            Product = new Widget();
            return this;
        }
        public Widget ReleaseProduct()
        {
            return Product;
        }
        public WidgetBuilder BuildColor(string color)
        {
            Product.Color = color;
            return this;
        }
        public WidgetBuilder BuildOptions(IWidgetOptions options)
        {
            Product.Options.Options.AddRange(options.Options);
            return this;
        }
        public WidgetBuilder BuildOption(string option)
        {
            Product.Options.Options.Add(option);
            return this;
        }
        public WidgetBuilder BuildHeader(string header)
        {
            Product.Header = header;
            return this;
        }
        public WidgetBuilder BuildFooter(string footer)
        {
            Product.Footer = footer;
            return this;
        }
        public WidgetBuilder BuildSubWidget(ISubWidget subwidget)
        {
            Product.Body = subwidget;
            return this;
        }
    }
}
