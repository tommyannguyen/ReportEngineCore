using Reporting.ChromiumRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            await ExportPdf();
            Assert.True(true);
        }
        [Fact]
        public void StressTest()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            int iterations = 100;
            var htmlFiles = new List<string>() {
                "htmls\\basic.html",
                "htmls\\interpolation-modes.html",
                "htmls\\line-styles.html",
                "htmls\\multi-axis.html",
                "htmls\\point-sizes.html",
                "htmls\\point-styles.html",
                "htmls\\skip-points.html",
                "htmls\\stepped.html"
            };
            Parallel.ForEach(Enumerable.Range(0, iterations), async (s, state, i) =>
                {
                    try
                    {
                        var htmlFileName = htmlFiles[s % htmlFiles.Count];
                        await ExportPdf($"Results\\{i}.pdf", htmlFileName);
                    }
                    catch(Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Assert.True(false);
                    }
                }
            );
            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Trace.WriteLine(elapsedMs + "ms");
            Assert.True(true);
        }

        [Fact]
        public async Task TestProcessHelper()
        {
            await ProcessAsyncHelper.ExecuteShellCommand("nodejs.exe", ".\\private_module\\index.js tt ss", 10000);
            Assert.True(true);
        }
        private async Task ExportPdf(string pdfFileName = "ExportHtmlAsync.pdf" , string htmlFileName = "htmls\\basic.html")
        {
            // factory return context
            using (var reportContext = CreateReportContext())
            {
                var html2Pdf = new DinkToPdfRepository();
                IHtmlRenderTask viewEngine = new FluidRenderTask(reportContext);
                IHtmlRenderTask preMailerEngine = new HtmlJsCssCleanupEngine(OutPutDirectory, reportContext);

                var chromiumEngineRepositoty = new ChromiumRepositoty(reportContext);
                var model = new { Name = "Con bướm xinh", Job = 100 };


                var htmlBody = await File.ReadAllTextAsync(htmlFileName);

                var body = await viewEngine.RenderAsync(model, htmlBody);
                body = await preMailerEngine.RenderAsync(model, body);

                File.WriteAllBytes(pdfFileName, chromiumEngineRepositoty.ExportFromHtml(body));
            }
        }
        private ReportContext CreateReportContext()
        {
            return new ReportContext(_logFactory, "D:\\Temp");
        }
    }
}
