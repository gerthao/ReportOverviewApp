using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportOverviewApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;

namespace ReportOverviewApp.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DataController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public enum FileExtension
        {
            Text, Csv, Json, Xml, Pdf, Excel
        }
        private async Task<JsonResult> ExportAsJson()
        {
            return Json(await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.Deadline == DateTime.Today).Select(rd => new
            {
                id = rd.Id,
                reportId = rd.ReportId,
                name = rd.Report.Name,
                deadline = rd.Deadline,
                runDate = rd.RunDate,
                approvalDate = rd.ApprovalDate,
                sentDate = rd.SentDate,
                instructions = rd.Report.WorkInstructions,
                notes = rd.Report.Notes
            }).ToListAsync());
        }
        public async Task<IActionResult> Export(FileExtension fileAs = FileExtension.Json)
        { 
            if(fileAs == FileExtension.Json)
            {
                return await ExportAsJson();
            }
            string sWebRootFolder = _hostingEnvironment.WebRootPath; 
            string sFileName = @"export.xlsx"; 
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName); 
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream(); 
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write)) 
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet("Reports");
                IRow row = excelSheet.CreateRow(0); 
 
                row.CreateCell(0).SetCellValue("Id"); 
                row.CreateCell(1).SetCellValue("ReportId");
                row.CreateCell(2).SetCellValue("Name");
                row.CreateCell(3).SetCellValue("Deadline");
                row.CreateCell(4).SetCellValue("RunDate");
                row.CreateCell(5).SetCellValue("ApprovalDate");
                row.CreateCell(6).SetCellValue("SentDate");


                var reportDeadlines = await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.Deadline == DateTime.Today).ToListAsync();
                for(int i = 0; i < reportDeadlines.Count(); i++)
                {
                    row = excelSheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(reportDeadlines[i].Id);
                    row.CreateCell(1).SetCellValue(reportDeadlines[i].ReportId);
                    row.CreateCell(2).SetCellValue(reportDeadlines[i].Report.Name);
                    row.CreateCell(3).SetCellValue(reportDeadlines[i].Deadline);
                    if(reportDeadlines[i].RunDate != null)
                        row.CreateCell(4).SetCellValue(reportDeadlines[i].RunDate.Value);
                    if (reportDeadlines[i].ApprovalDate != null)
                        row.CreateCell(5).SetCellValue(reportDeadlines[i].ApprovalDate.Value);
                    if (reportDeadlines[i].SentDate != null)
                        row.CreateCell(6).SetCellValue(reportDeadlines[i].SentDate.Value);
                }
                workbook.Write(fs); 
            } 
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open)) 
            { 
                await stream.CopyToAsync(memory); 
            } 
            memory.Position = 0; 
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName); 
        } 

        /// <summary>
        ///  Gets a Report based on its ID in JSON format.
        /// </summary>
        /// <param name="id">
        ///  "id" must be a positive integer.
        /// </param>
        /// <returns>
        ///  Returns JsonResult containing a JSON-formatted report.
        /// </returns>
        [Authorize]
        public async Task<JsonResult> GetReport(int? id) => Json(await _context.Reports.Where(r => r.Id == id).ToListAsync());
        [Authorize]
        public async Task<JsonResult> GetReports(int? id_1, int? id_2) => Json(await _context.Reports.Where(r => r.Id >= id_1 && r.Id <= id_2).OrderBy(r => r.Id).ToListAsync());
        [Authorize]
        public async Task<JsonResult> GetAllReports() => Json(await _context.Reports.OrderBy(r => r.Name).ToListAsync());

        //[Authorize]
        //public async Task<JsonResult> GetGraphData()
        //{
        //    List<DateTime> dates = new List<DateTime>();
        //    var i = await _context.Reports.Select()
        //}

        /// <summary>
        /// Returns JSON data of report names and deadlines.
        /// </summary>
        /// <param name="days">
        /// Default is null which returns all ReportFragments.  Else returns the ReportFragments that have a deadline on days added onto the current date.
        /// </param>
        /// <returns>Returns JSON data of report names and deadlines.</returns>
        [Authorize]
        public async Task<JsonResult> GetDeadlines(double? days)
        {
            if (days == null)
            {
                return Json(await _context.Reports.OrderBy(r => r.Name).Select(r => new ReportFragment(r)).ToListAsync());
            }
            return Json(await _context.Reports.OrderBy(r => r.Name).Select(r => new ReportFragment(r)).Where(r => r.ReportDeadline == DateTime.Today.AddDays(days.Value)).ToListAsync());
        }

        /// <summary>
        /// Returs JSON-formatted data of UserLogs.
        /// </summary>
        /// <returns>
        /// Returs JSON-formatted data of UserLogs.
        /// </returns>
        [Authorize]
        public async Task<JsonResult> GetUserLogs()
        {
            return Json(await _context.UserLogs.OrderByDescending(ul => ul.TimeStamp).ToListAsync());
        }
        public class ReportFragment
        {
            public string ReportName { get; private set; }
            public DateTime? ReportDeadline { get; private set; }
            public ReportFragment(Report report)
            {
                ReportName = report.Name;
                ReportDeadline = report.CurrentDeadline();
            }
        }

        //[Authorize]
        //public async Task<JsonResult> GetCurrentDateTime()
        //{
        //    DateTime dateTime = DateTime.Now;
        //    return Json(await Task.FromResult(dateTime.ToShortTimeString()));
        //}
        
    }
}
