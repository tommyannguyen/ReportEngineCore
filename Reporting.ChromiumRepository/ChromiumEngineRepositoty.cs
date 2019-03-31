using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Reporting.ChromiumRepository
{
    public class ChromiumRepositoty: IHtml2Pdf
    {
        private readonly ILogger _logger;
        private readonly ReportContext _reportContext;
        public ChromiumRepositoty(ReportContext reportContext)
        {
            _reportContext = reportContext;
            _logger = reportContext.LoggerFactory.CreateLogger<ChromiumRepositoty>();
        }
        /// <summary>
        /// Finshed render html track by add element id = my_reporting_finished
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public byte[] ExportFromHtml(string html)
        {
            var sectionFileId = Guid.NewGuid();
            var htmlPath = $"{_reportContext.GetSessionPath()}\\{sectionFileId}.html";
            File.WriteAllText(htmlPath, html);
            var pdfFilePath = $"{_reportContext.GetSessionPath()}\\{sectionFileId}.pdf";
 
            string log = RunProgress(htmlPath, pdfFilePath);
            if (File.Exists(pdfFilePath))
            {
                _logger.LogInformation(log);
                _reportContext.AddTempFiles("ChromiumRepositoty", new List<string> { htmlPath, pdfFilePath });
                return File.ReadAllBytes(pdfFilePath);
            }
            _logger.LogError(log);
            _reportContext.AddTempFiles("ChromiumRepositoty", new List<string> { htmlPath });
            throw new Exception("Cant translate html to pdf !");
        }

        private static object _lock = new object();
        private static string RunProgress(string htmlPath, string pdfFilePath)
        {
            lock (_lock)
            {
                using (var cmd = new Process())
                {
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;

                    cmd.Start();
                    cmd.StandardInput.WriteLine($"node .\\private_module\\index.js {htmlPath} {pdfFilePath}");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();

                    return cmd.StandardOutput.ReadToEnd();
                }
            }
        }

        public byte[] ExportFromUri(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
