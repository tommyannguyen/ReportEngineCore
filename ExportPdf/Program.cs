using Reporting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExportPdf
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var html2Pdf = new Html2Pdf();
            var viewEngine = new HtmlViewEngine();
            var htmlBody = await File.ReadAllTextAsync($"Views\\index.html");
            var body = await viewEngine.RenderAsync(new { Name = "Con bướm xinh", Job = 100 }, htmlBody);
            File.WriteAllBytes("tententen.pdf", html2Pdf.ExportFromHtml(body));
        }
    }
}
