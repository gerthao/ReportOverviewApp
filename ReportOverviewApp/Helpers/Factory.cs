using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Helpers
{
    /// <summary>
    ///  Abstract class to be used for other classes wanting to use the Factory Design Pattern.  Not fully implemented.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Factory<T>
    {
        protected List<T> Products;
        /// <summary>
        ///  Gets an object of type T 
        /// </summary>
        /// <returns>
        ///  Returns an object of type T
        /// </returns>
        public abstract T Build();
        public abstract bool HasProduct(T item);
        protected abstract T Retrieve(T item);
        public abstract void Register(T item);
    }
}
