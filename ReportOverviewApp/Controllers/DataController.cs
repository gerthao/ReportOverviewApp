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
using System.Text;

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
        
        private async Task<JsonResult> ExportAsJson(string fileName, DateTime? begin, DateTime? end)
        {
            if (begin == null || !begin.HasValue)
            {
                begin = DateTime.Today;
            }
            if (end == null || !end.HasValue)
            {
                end = DateTime.Today;
            }
            return Json(await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.Deadline >= begin && rd.Deadline <= end).Select(rd => new
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
        private async Task<IActionResult> ExportAsExcel(string fileName, DateTime? begin, DateTime? end)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string URL;

            FileInfo file;
            try
            {
                fileName = $@"{fileName}.xlsx";
                URL = $"{Request.Scheme}://{Request.Host}/{fileName}";
                file = new FileInfo(Path.Combine(webRootPath, fileName));
            }
            catch (NotSupportedException)
            {
                fileName = $@"reportdeadlines_exported_({DateTime.Now.ToString("MM.dd.yyyy hh.mm.ss tt")}).xlsx";
                URL = $"{Request.Scheme}://{Request.Host}/{fileName}";
                file = new FileInfo(Path.Combine(webRootPath, fileName));
            }

            MemoryStream memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Create, FileAccess.Write))
            {
                var reportDeadlines = await _context.ReportDeadlines.Include(rd => rd.Report)
                                                                    .ThenInclude(r => r.ReportPlanMapping)
                                                                        .ThenInclude(rpm => rpm.Plan)
                                                                    .Where(rd => rd.Deadline >= begin && rd.Deadline <= end).ToListAsync();
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Reports");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Id");
                row.CreateCell(1).SetCellValue("ReportId");
                row.CreateCell(2).SetCellValue("Name");
                row.CreateCell(3).SetCellValue("Plan(s)");
                row.CreateCell(4).SetCellValue("Deadline");
                row.CreateCell(5).SetCellValue("RunDate");
                row.CreateCell(6).SetCellValue("ApprovalDate");
                row.CreateCell(7).SetCellValue("SentDate");
                for (int i = 0; i < reportDeadlines.Count(); i++)
                {
                    row = excelSheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(reportDeadlines[i].Id);
                    row.CreateCell(1).SetCellValue(reportDeadlines[i].ReportId);
                    row.CreateCell(2).SetCellValue(reportDeadlines[i].Report.Name);
                    if (reportDeadlines[i].Report.ReportPlanMapping != null && reportDeadlines[i].Report.ReportPlanMapping.Select(rpm => rpm.Plan).Any())
                    {
                        row.CreateCell(3).SetCellValue(reportDeadlines[i].Report.ReportPlanMapping.Select(rpm => rpm.Plan.Name).Aggregate((a, b) => $"{a}, {b}"));
                    }
                    row.CreateCell(4).SetCellValue(reportDeadlines[i].Deadline.ToString("MM/dd/yyyy"));
                    if (reportDeadlines[i].RunDate != null)
                    {
                        row.CreateCell(5).SetCellValue(reportDeadlines[i].RunDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    }
                    if (reportDeadlines[i].ApprovalDate != null)
                    {
                        row.CreateCell(6).SetCellValue(reportDeadlines[i].ApprovalDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    }
                    if (reportDeadlines[i].SentDate != null)
                    {
                        row.CreateCell(7).SetCellValue(reportDeadlines[i].SentDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    }
                }
                workbook.Write(fileStream);
            }
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        public async Task<IActionResult> ExportReportDeadlines(DateTime? begin, DateTime? end, string fileName, FileExtension fileAs = FileExtension.Excel)
        {
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrWhiteSpace(fileName))
            {
                fileName = fileName.Replace("\\", "")
                            .Replace("/", "")
                            .Replace("\"", "")
                            .Replace("*", "")
                            .Replace(":", "")
                            .Replace("?", "")
                            .Replace("<", "")
                            .Replace(">", "")
                            .Replace("|", "")
                            .Trim();
            }
            if (begin == null || !begin.HasValue)
            {
                begin = DateTime.Today;
            }
            if(end == null || !end.HasValue)
            {
                end = DateTime.Today;
            }
            switch (fileAs)
            {
                case FileExtension.Json:
                    return await ExportAsJson(fileName, begin, end);
                case FileExtension.Csv:
                    return await ExportAsCsv(fileName, begin, end);
                case FileExtension.Xml:
                    return await ExportAsXml(fileName, begin, end);
                case FileExtension.Excel:
                    return await ExportAsExcel(fileName, begin, end);
            }
            return View("Error");
        }

        private Task<IActionResult> ExportAsXml(string fileName, DateTime? begin, DateTime? end)
        {
            throw new NotImplementedException();
        }

        private async Task<IActionResult> ExportAsCsv(string fileName, DateTime? begin, DateTime? end)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string URL;

            FileInfo file;
            try
            {
                fileName = $@"{fileName}.csv";
                URL = $"{Request.Scheme}://{Request.Host}/{fileName}";
                file = new FileInfo(Path.Combine(webRootPath, fileName));
            }
            catch (NotSupportedException)
            {
                fileName = $@"reportdeadlines_exported_({DateTime.Now.ToString("MM.dd.yyyy hh.mm.ss tt")}).csv";
                URL = $"{Request.Scheme}://{Request.Host}/{fileName}";
                file = new FileInfo(Path.Combine(webRootPath, fileName));
            }

            MemoryStream memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Create, FileAccess.Write))
            {
                var reportDeadlines = await _context.ReportDeadlines.Include(rd => rd.Report)
                                                                    .ThenInclude(r => r.ReportPlanMapping)
                                                                        .ThenInclude(rpm => rpm.Plan)
                                                                    .Where(rd => rd.Deadline >= begin && rd.Deadline <= end).ToListAsync();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Id,")
                             .Append("ReportId,")
                             .Append("Name,")
                             .Append("Plans,")
                             .Append("Deadline,")
                             .Append("RunDate,")
                             .Append("ApprovalDate,")
                             .AppendLine("SentDate,");

                foreach(ReportDeadline reportDeadline in reportDeadlines)
                {
                    stringBuilder.Append($"{reportDeadline.Id},")
                             .Append($"{reportDeadline.ReportId},")
                             .Append($"{reportDeadline.Report.Name},");
                    if(reportDeadline.Report.ReportPlanMapping != null && reportDeadline.Report.ReportPlanMapping.Select(rpm => rpm.Plan).Any())
                    {
                        stringBuilder.Append($"{reportDeadline.Report.ReportPlanMapping.Select(rpm => rpm.Plan.Name).Aggregate((a, b) => $"\"{a}, {b}\"")},");
                    }
                    else
                    {
                        stringBuilder.Append($"\"\",");
                    }
                    stringBuilder.Append($"{reportDeadline.Deadline},");
                    if (reportDeadline.RunDate != null)
                    {
                        stringBuilder.Append($"{reportDeadline.RunDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt")},");
                    }
                    else
                    {
                        stringBuilder.Append($"\"\",");
                    }
                    if (reportDeadline.ApprovalDate != null)
                    {
                        stringBuilder.Append($"{reportDeadline.ApprovalDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt")},");
                    }
                    else
                    {
                        stringBuilder.Append($"\"\",");
                    }
                    if (reportDeadline.SentDate != null)
                    {
                        stringBuilder.AppendLine(reportDeadline.SentDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    }
                    else
                    {
                        stringBuilder.AppendLine($"\"\"");
                    }
                }
                //line needed//
            }
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
