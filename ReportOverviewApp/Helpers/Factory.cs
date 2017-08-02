using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Helpers
{
    public abstract class CustomFactory<T>
    {
        protected List<T> Products;
        public abstract T Build();
        public abstract bool HasProduct(T item);
        protected abstract T Retrieve(T item);
        public abstract void Register(T item);
    }
}
