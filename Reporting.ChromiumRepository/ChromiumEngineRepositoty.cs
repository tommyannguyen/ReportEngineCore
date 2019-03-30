using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace Reporting.ChromiumRepository
{
    public class ChromiumEngineRepositoty: IHtml2Pdf
    {
        private readonly string _tempPath;
        private readonly ILogger _logger;

        public ChromiumEngineRepositoty(ILoggerFactory loggerFactory, string tempPath)
        {
            _tempPath = tempPath;
            _logger = loggerFactory.CreateLogger<ChromiumEngineRepositoty>();
        }
        public byte[] ExportFromHtml(string html)
        {
            var sectionId = Guid.NewGuid();
            var htmlPath = $"{_tempPath}\\{sectionId}.html";
            File.WriteAllText(htmlPath, html);
            var pdfFilePath = $"{_tempPath}\\{sectionId}.pdf";

            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            cmd.StandardInput.WriteLine("cd private_module");
            cmd.StandardInput.WriteLine($"node index.js {htmlPath} {pdfFilePath}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            var log = cmd.StandardOutput.ReadToEnd();
            if(File.Exists(pdfFilePath))
            {
                _logger.LogInformation(log);
                return File.ReadAllBytes(pdfFilePath);
            }
            _logger.LogError(log);
            throw new Exception("Cant translate html to pdf !");
        }

        public byte[] ExportFromUri(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
