using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using ReportOverviewApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

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
        [Produces("application/json")]
        private async Task<JsonResult> ExportAsJson(DateTime? begin, DateTime? end)
        {
            if (begin == null || !begin.HasValue)
            {
                begin = DateTime.Today;
            }
            if (end == null || !end.HasValue)
            {
                end = DateTime.Today;
            }
            //var ser = JsonSerializer.Create();
            //ser.Serialize(new StreamWriter(new MemoryStream()), null);
            return Json(await GetExportedDataXmlAsync(begin, end), new JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            });
        }
        public class ReportExportData
        {
            public int ReportDeadlineId { get; set; }
            public DateTime Deadline { get; set; }
            public DateTime? RunDate { get; set; }
            public DateTime? ApprovalDate { get; set; }
            public DateTime? SentDate { get; set; }
            public int ReportId { get; set; }
            public string ReportName { get; set; }
            public string Frequency { get; set; }
            public List<string> Plans { get; set; }
        }
        private async Task<List<ReportExportData>> GetExportedDataXmlAsync(DateTime? begin, DateTime? end)
        {
            var export = await _context.ReportDeadlines.Include(rd => rd.Report)
                                .ThenInclude(r => r.ReportPlanMapping)
                                .ThenInclude(rpm => rpm.Plan)
                                .Where(rd => rd.Deadline >= begin && rd.Deadline <= end)
                                .Select(rd => new ReportExportData()
                                {
                                    ReportDeadlineId = rd.Id,
                                    Deadline = rd.Deadline,
                                    RunDate = rd.RunDate,
                                    ApprovalDate = rd.ApprovalDate,
                                    SentDate = rd.SentDate,
                                    ReportId = rd.ReportId,
                                    ReportName = rd.Report.Name ?? rd.Report.OtherReportName ?? String.Empty,
                                    Frequency = rd.Report.Frequency,
                                    Plans = rd.Report.ReportPlanMapping.Where(rpm => rpm.Plan != null && !String.IsNullOrEmpty(rpm.Plan.Name)).Select(rpm => rpm.Plan.Name).ToList()
                                })
                                .OrderBy(rd => rd.ReportId).ToListAsync();
            return export;
            
        }

        private async Task<List<Dictionary<string, object>>> GetExportedDataAsync(DateTime? begin, DateTime? end) =>
            await _context.ReportDeadlines.Include(rd => rd.Report)
                                                .ThenInclude(r => r.ReportPlanMapping)
                                                    .ThenInclude(rpm => rpm.Plan)
                                                .Where(rd => rd.Deadline >= begin && rd.Deadline <= end)
                                                .Select(rd => new
                                                {
                                                    ReportDeadlineId = rd.Id,
                                                    rd.Deadline,
                                                    rd.RunDate,
                                                    rd.ApprovalDate,
                                                    rd.SentDate,
                                                    ReportName = rd.Report.Name ?? rd.Report.OtherReportName ?? String.Empty,
                                                    rd.ReportId,
                                                    rd.Report.Frequency,
                                                    Plans = rd.Report.ReportPlanMapping.Select(rpm => rpm.Plan) ?? null
                                                })
                                                .OrderBy(r => r.ReportId)
                                                .Select(rd => rd.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToDictionary(f => f.Name, f => f.GetValue(rd))).ToListAsync();
        
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
                fileName = $@"exported_({DateTime.Now.ToString("MM.dd.yyyy hh.mm.ss tt")}).xlsx";
                URL = $"{Request.Scheme}://{Request.Host}/{fileName}";
                file = new FileInfo(Path.Combine(webRootPath, fileName));
            }
            MemoryStream memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Create, FileAccess.Write))
            {
                var exportData = await GetExportedDataAsync(begin, end);
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet($"Reports");
                IRow row = excelSheet.CreateRow(0);
                if(exportData.Count() == 0)
                {
                    row.CreateCell(0).SetCellValue("No Data");
                }
                else
                {
                    var keys = exportData.ElementAt(0).Keys;
                    for (int i = 0; i < keys?.Count(); i++)
                    {
                        row.CreateCell(i).SetCellValue(keys?.ElementAt(i).Split('>')[0].Replace("<", String.Empty));
                    }
                    for (int i = 0; i < exportData.Count(); i++)
                    {
                        row = excelSheet.CreateRow(i + 1);
                        for (int j = 0; j < exportData.ElementAt(i).Count(); j++)
                        {
                            if(exportData.ElementAt(i).ElementAt(j).Value is IEnumerable<Plan>)
                            {
                                if(exportData.ElementAt(i).ElementAt(j).Value != null && (exportData.ElementAt(i).ElementAt(j).Value as IEnumerable<Plan>).Any())
                                {
                                    row.CreateCell(j).SetCellValue((exportData.ElementAt(i).ElementAt(j).Value as IEnumerable<Plan>).Select(p => p.Name).Aggregate((a, b) => $"{a},{b}") ?? null);
                                } else
                                {
                                    row.CreateCell(j).SetCellValue(String.Empty);
                                }    
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(exportData.ElementAt(i).ElementAt(j).Value?.ToString());
                            }
                           
                        }
                    }
                }
                workbook.Write(fileStream);
            }
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        public async Task<IActionResult> ExportReportDeadlines(DateTime? begin, DateTime? end, string fileName, FileExtension fileAs = FileExtension.Excel)
        {
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrWhiteSpace(fileName))
            {
                fileName = fileName.Replace("\\", "")
                            .Replace("/", "-")
                            .Replace("\"", "-")
                            .Replace("*", "")
                            .Replace(":", ".")
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
                    return await ExportAsJson(begin, end);
                case FileExtension.Csv:
                    return await ExportAsSeparatedValues(fileName, ',', begin, end);
                case FileExtension.Tsv:
                    return await ExportAsSeparatedValues(fileName, '\t', begin, end);
                case FileExtension.Xml:
                    return await ExportAsXml(fileName, begin, end);
                case FileExtension.Excel:
                    return await ExportAsExcel(fileName, begin, end);
            }
            return View("Error");
        }

        private async Task<IActionResult> ExportAsXml(string fileName, DateTime? begin, DateTime? end)
        {
            fileName = $@"{fileName}.xml";
            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                var exportData = await GetExportedDataXmlAsync(begin, end);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ReportExportData>));
                xmlSerializer.Serialize(streamWriter, exportData);
            }
            return File(memoryStream.GetBuffer(), "text/xml", fileName);
        }
        /// <summary>
        /// not fully implemented
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        private string HandleData(object data, char delimiter)
        {
            if(data is IEnumerable<Plan>)
            {
                var plans = (data as IEnumerable<Plan>);
                if(plans.Select(p => p.Name).Any())
                {
                    return plans.Select(p => p.Name).Aggregate((a, b) => $"{a}{delimiter}{b}");
                }
            }
            return String.Empty;
        }
        private async Task<IActionResult> ExportAsSeparatedValues(string fileName, char delimiter, DateTime? begin, DateTime? end)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string extension = delimiter == ',' ? "csv" : delimiter == '\t' ? "tsv" : "file";
            string mediaType = delimiter == ',' ? "text/csv" : delimiter == '\t' ? "text/tab-separated-values" : "text/plain";
            fileName = $@"{fileName}.{extension}";
            MemoryStream memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                var exportData = await GetExportedDataAsync(begin, end);
                if (exportData.Count() > 0)
                {
                    var keys = exportData.ElementAt(0).Keys;
                    await streamWriter.WriteLineAsync(keys?.Select(k => k.Split('>')[0].Replace("<", String.Empty)).Aggregate((a, b) => $"{a}{delimiter}{b}"));
                    exportData.ForEach(async dictionary => await streamWriter.WriteLineAsync(dictionary.Select(kv => kv.Value == null ? String.Empty : kv.Value is IEnumerable<Plan> ? ((kv.Value as IEnumerable<Plan>).Select(p => p.Name).Any() ? (kv.Value as IEnumerable<Plan>).Select(p => p.Name).Aggregate((a, b) => $"{a.Replace(",", String.Empty)};{b.Replace(",", String.Empty)}") : String.Empty) : kv.Value.ToString()).Aggregate((a, b) => $"{a.Replace(",", String.Empty)}{delimiter}{b.Replace(",", String.Empty)}")));
                }
                await streamWriter.FlushAsync();
            }
            return File(memoryStream.GetBuffer(), mediaType, fileName);
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

        [Authorize]
        public async Task<JsonResult> GetDeadlinesCount(double? days)
        {
            if (days == null)
            {
                return Json(await _context.ReportDeadlines.Include(rd => rd.Report).OrderBy(rd => rd.Deadline).ThenBy(rd => rd.Report.Name).GroupBy(rd => rd.Deadline).ToDictionaryAsync(k => k.Key.ToString("yyyy-MM-dd"), v => v.Count()));
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
