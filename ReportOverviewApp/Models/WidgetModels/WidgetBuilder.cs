using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class WidgetBuilder
    {
        Widget Product;
        private void CheckProduct(ref Widget product)
        {
            if(product == null)
            {
                product = new Widget();
            } if(product.Body == null)
            {
                product.Body = new SubWidget();
            } if(product.Options == null)
            {
                product.Options = new WidgetOptions();
            }
        }
        public WidgetBuilder BuildID(int ID)
        {
            Product.ID = ID;
            return this;
        }
        public WidgetBuilder BuildProduct()
        {
            Product = new Widget();
            CheckProduct(ref Product);
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
            if(Product.Options == null)
            {
                Product.Options = new WidgetOptions();
            }
            Product.Options.Options.AddRange(options.Options);
            return this;
        }
        public WidgetBuilder BuildOption(string option)
        {
            if (Product.Options == null)
            {
                Product.Options = new WidgetOptions();
            }
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
            Product.Body = (SubWidget) subwidget;
            return this;
        }
        public WidgetBuilder BuildSubWidgetTopic(string topic)
        {
            if(Product.Body == null)
            {
                Product.Body = new SubWidget();
            }
            Product.Body.Topic = topic;
            return this;
        }
        public WidgetBuilder BuildSubWidgetAction(Func<ApplicationDbContext, int> function)
        {
            Product.Body.Action = function;
            return this;
        }
        //public WidgetBuilder BuildSubWidgetAction(Func<ApplicationDbContext, List<string>> function)
        //{
        //    Product.Body.Action = function;
        //    return this;
        //}
        public WidgetBuilder BuildSubWidgetDescription(string description)
        {
            Product.Body.Description = description;
            return this;
        }
    }
}
