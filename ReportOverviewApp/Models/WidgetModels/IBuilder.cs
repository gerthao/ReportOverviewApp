using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    /// <summary>
    /// An interface for the builder design pattern.
    /// </summary>
    public interface IBuilder
    {
        IBuilder BuildProduct();
        object ReleaseProduct();
    }
}
