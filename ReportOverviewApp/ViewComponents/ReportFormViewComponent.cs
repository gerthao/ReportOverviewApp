using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.ViewComponents
{
    public class ReportFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string form)
        {
            if(String.IsNullOrEmpty(form) || String.IsNullOrWhiteSpace(form))
            {
                form = "create";
            }
            switch (form.ToLower())
            {
                case "create":
                    return View("Create", new Report());
                case "manage":
                    return View("Manage", new Report());
            }
            return View("Error");
        }
    }
}
