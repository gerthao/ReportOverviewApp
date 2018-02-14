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
using Microsoft.AspNetCore.Mvc.Rendering;

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
        /// <summary>
        ///  Exports report deadline information into JSON.
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
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
        private async Task<List<Dictionary<string, object>>> GetExportedPlanData()
        {
            var data = await _context.Plans.Include(p => p.ReportPlanMapping).ThenInclude(rpm => rpm.Report)
                           .Select(p => new
                           {
                               p.Name,
                               ReportCount = p.ReportPlanMapping.Count()

                           }).ToListAsync();
            data.Add(new { Name = "Total", ReportCount = _context.Reports.Count() });
            return data.OrderBy(p => p.Name).Select(p => p.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToDictionary(f => f.Name, f => f.GetValue(p))).ToList();
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

        private async Task<IActionResult> ExportAsExcel(string fileName, List<Dictionary<string, object>> exportData)
        {
            fileName = $"{fileName}.xlsx";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string URL = $"{Request.Scheme}://{Request.Host}/{fileName}";
            FileInfo file = new FileInfo(Path.Combine(webRootPath, fileName));
            MemoryStream memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(Path.Combine(webRootPath, fileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet($"Reports");
                IRow row = excelSheet.CreateRow(0);
                if (exportData.Count() > 0)
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
                            if (exportData.ElementAt(i).ElementAt(j).Value is IEnumerable<Plan>)
                            {
                                if (exportData.ElementAt(i).ElementAt(j).Value != null && (exportData.ElementAt(i).ElementAt(j).Value as IEnumerable<Plan>).Any())
                                {
                                    row.CreateCell(j).SetCellValue((exportData.ElementAt(i).ElementAt(j).Value as IEnumerable<Plan>).Select(p => p.Name).Aggregate((a, b) => $"{a};{b}") ?? null);
                                }
                                else
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
            file.Delete();
            return File(memoryStream.GetBuffer(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        /// <summary>
        ///  Takes out invalid file name characters in a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string StripInvalidFileNameCharacters(string input) =>
            input.Replace("\\", String.Empty)
                .Replace("/", String.Empty)
                .Replace("\"", String.Empty)
                .Replace("*", String.Empty)
                .Replace(":", String.Empty)
                .Replace("?", String.Empty)
                .Replace("<", String.Empty)
                .Replace(">", String.Empty)
                .Replace("|", String.Empty)
                .Trim();

        /// <summary>
        ///  Exports report deadline information into a file.
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="fileName"></param>
        /// <param name="fileAs"></param>
        /// <returns>
        /// 
        /// </returns>
        public async Task<IActionResult> ExportReportDeadlines(DateTime? begin, DateTime? end, string fileName, FileExtension fileAs = FileExtension.Excel)
        {
            if (begin == null || !begin.HasValue)
            {
                begin = DateTime.Today;
            }
            if (end == null || !end.HasValue)
            {
                end = DateTime.Today;
            }
            if (string.IsNullOrEmpty(fileName) && string.IsNullOrWhiteSpace(fileName))
            {
                fileName = $"ReportDeadlines From {begin.Value.ToString("MM-dd-yyyy")} To {end.Value.ToString("MM-dd-yyyy")}";
            }
            fileName = StripInvalidFileNameCharacters(fileName);
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
                    return await ExportAsExcel(fileName, await GetExportedDataAsync(begin, end));
            }
            return View("Error");
        }

        public async Task<IActionResult> ExportPlans(string fileName, FileExtension fileAs = FileExtension.Excel)
        {
            if (string.IsNullOrEmpty(fileName) && string.IsNullOrWhiteSpace(fileName))
            {
                fileName = $"Report Count By Plans";
            }
            fileName = StripInvalidFileNameCharacters(fileName);
            switch (fileAs)
            {
                case FileExtension.Excel:
                    return await ExportAsExcel(fileName, await GetExportedPlanData());
            }
            return View("Error");
        }

        /// <summary>
        ///  Exports report deadline information into an XML file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
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
            if (data is IEnumerable<Plan>)
            {
                var plans = (data as IEnumerable<Plan>);
                if (plans.Select(p => p.Name).Any())
                {
                    return plans.Select(p => p.Name).Aggregate((a, b) => $"{a}{delimiter}{b}");
                }
            }
            return String.Empty;
        }
        /// <summary>
        ///  Exports report deadline information into a delimited file (CSV or TSV).
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private async Task<IActionResult> ExportAsSeparatedValues(string fileName, char delimiter, DateTime? begin, DateTime? end)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string extension = delimiter == ',' ? "csv" : delimiter == '\t' ? "txt" : "file";
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
                    exportData.ForEach(async dictionary =>
                        await streamWriter.WriteLineAsync(
                            dictionary.Select(
                                kv => kv.Value == null ? String.Empty :
                                    kv.Value is IEnumerable<Plan> ? ((kv.Value as IEnumerable<Plan>).Select(p => p.Name).Any() ? $"\"{(kv.Value as IEnumerable<Plan>).Select(p => p.Name).Aggregate((a, b) => $"{a};{b}")}\"" : String.Empty) : kv.Value.ToString()).Aggregate((a, b) => $"{a}{delimiter}{b.Replace(delimiter.ToString(), String.Empty)}")));
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
        public async Task<JsonResult> GetAllReports(bool indent, bool omitNull)
        {
            var reports = await _context.Reports.OrderBy(e => e.Id).ToListAsync();
            return Json(reports, new JsonSerializerSettings() {Formatting = indent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None, NullValueHandling = omitNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }
        [Authorize]
        public async Task<JsonResult> GetBusinessContactReports(int? id, bool indent, bool omitNull)
        {
            Dictionary<int, string> reports;
            if (id == null || !id.HasValue || id == 0)
            {
                reports = await _context.Reports.Where(r => r.BusinessContactId == null).ToDictionaryAsync(r => r.Id, r => r.Name);
            }
            else
            {
                reports = await _context.Reports.Where(r => r.BusinessContactId == id).ToDictionaryAsync(r => r.Id, r => r.Name);
            }
            return Json(reports, new JsonSerializerSettings() { Formatting = indent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None, NullValueHandling = omitNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }
        [Authorize]
        public async Task<SelectList> GetBusinessContactReportSelectList(int? id)
        {
            var reports = new SelectList(await _context.Reports.Where(r => r.BusinessContactId == id).Select(r => new { value = r.Id, text = r.Name }).ToListAsync(), "value", "text");
            return reports;
        }
        private string GetCurrentUserID() => _context.Users.Where(u => u.UserName.Equals(User.Identity.Name)).SingleOrDefault().Id;
    }
}
