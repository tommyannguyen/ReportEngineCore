using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Reporting.Test
{
    public class ExportPdfTest
    {
        [Fact]
        public async Task ExportPdfFromEngineAsync()
        {
            var html2Pdf = new Html2Pdf();
            var viewEngine = new FluidHtmlViewEngine();
            var htmlBody = await File.ReadAllTextAsync($"Views\\index.html");
            var body = await viewEngine.RenderAsync(new { Name = "Con bướm xinh", Job = 100 }, htmlBody);
            File.WriteAllBytes("tententen.pdf", html2Pdf.ExportFromHtml(body));
        }
    }
}
