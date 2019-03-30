using Reporting.ChromiumRepository;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Reporting.Test
{
    public class ExportPdfTest : TestBase
    {

        [Fact]
        public async Task ExportPdfFromEngineAsync()
        {
            // factory return context
            using (var reportContext = CreateReportContext())
            {
                var html2Pdf = new DinkToPdfRepository();
                IHtmlRenderTask viewEngine = new FluidRenderTask(reportContext);
                IHtmlRenderTask preMailerEngine = new HtmlJsCssCleanupEngine(OutPutDirectory, reportContext);
                var model = new { Name = "Con bướm xinh", Job = 100 };
                var htmlBody = await File.ReadAllTextAsync($"Views\\index.html");

                var body = await viewEngine.RenderAsync(model, htmlBody);
                body = await preMailerEngine.RenderAsync(model, body);

                File.WriteAllBytes("tententen.html", Encoding.UTF8.GetBytes(body));
                File.WriteAllBytes("tententen.pdf", html2Pdf.ExportFromHtml(body));
                Assert.True(true);
            }
        }


        [Fact]
        public async Task ExportChartJsHtmlAsync()
        {
            // factory return context
            using (var reportContext = CreateReportContext())
            {
                var html2Pdf = new DinkToPdfRepository();
                IHtmlRenderTask viewEngine = new FluidRenderTask(reportContext);
                IHtmlRenderTask preMailerEngine = new HtmlJsCssCleanupEngine(OutPutDirectory, reportContext);

                var chromiumEngineRepositoty = new ChromiumRepositoty(reportContext);
                var model = new { Name = "Con bướm xinh", Job = 100 };


                var htmlBody = await File.ReadAllTextAsync("htmls\\basic.html");

                var body = await viewEngine.RenderAsync(model, htmlBody);
                body = await preMailerEngine.RenderAsync(model, body);
                
                File.WriteAllBytes("ExportHtmlAsync.pdf", chromiumEngineRepositoty.ExportFromHtml(body));
            }
            Assert.True(true);
        }

        private ReportContext CreateReportContext()
        {
            return new ReportContext(_logFactory, "D:\\Temp");
        }
    }
}
