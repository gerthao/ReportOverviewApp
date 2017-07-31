using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public static class WidgetFunctions
    {
        private static Func<Report, bool> Before(DateTime date) => r => r.Deadline() <= date;
        private static Func<Report, bool> After(DateTime date) => r => r.Deadline() >= date;
        //private static Func<Report, Func<Report, bool>, Func<Report, bool>, bool> Between(DateTime beginning, DateTime end)
        //{
        //    return (r, a, b) => r => a => b;
        //}
        public static Func<ApplicationDbContext, int> ReportCount() => context => context.Reports.Count();
        public static Func<ApplicationDbContext, int> ReportCount(DateTime deadline) => context => context.Reports.Where(Before(deadline)).Count();
        public static Func<ApplicationDbContext, int> ReportCount(DateTime beginning, DateTime end) => (x) => x.Reports.Where(After(beginning)).Where(Before(end)).Count();

        public class WidgetFunction
        {
            private Func<object, object> function;
            private WidgetFunction() { }
            private WidgetFunction(Func<object, object> f)
            {
                function = f;
            }
            public WidgetFunction Create(Func<object, object> f)
            {
                function = f;
                return this;
            }
            private Func<object, object> Apply() => function;
        }
    }
}
