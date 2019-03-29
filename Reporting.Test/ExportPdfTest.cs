using Reporting.HtmlEngine;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Reporting.Test
{
    public class ExportPdfTest : TestBase
    {
        public string OutPutDirectory => AppDomain.CurrentDomain.BaseDirectory;
        [Fact]
        public async Task ExportPdfFromEngineAsync()
        {
            var html2Pdf = new Html2Pdf();
            IHtmlRenderEngine viewEngine = new FluidHtmlViewEngine(_logFactory);
            IHtmlRenderEngine preMailerEngine = new HtmlJsCssCleanupEngine(OutPutDirectory, _logFactory);
            var model = new { Name = "Con bướm xinh", Job = 100 };
            var htmlBody = await File.ReadAllTextAsync($"Views\\index.html");

            var body = await viewEngine.RenderAsync(model, htmlBody);
            body = await preMailerEngine.RenderAsync(model, body);

            File.WriteAllBytes("tententen.html", Encoding.UTF8.GetBytes(body));
            File.WriteAllBytes("tententen.pdf", html2Pdf.ExportFromHtml(body));
            Assert.True(true);
        }


        [Fact]
        public async Task ExportHtmlAsync()
        {
            var html2Pdf = new Html2Pdf();
            IHtmlRenderEngine viewEngine = new FluidHtmlViewEngine(_logFactory);
            IHtmlRenderEngine preMailerEngine = new HtmlJsCssCleanupEngine(OutPutDirectory, _logFactory);
            var model = new { Name = "Con bướm xinh", Job = 100 };


            var htmlBody = await File.ReadAllTextAsync("htmls\\basic.html");

            var body = await viewEngine.RenderAsync(model, htmlBody);
            body = await preMailerEngine.RenderAsync(model, body);

            File.WriteAllBytes("ExportHtmlAsync.html", Encoding.UTF8.GetBytes(body));
            File.WriteAllBytes("ExportHtmlAsync.pdf", html2Pdf.ExportFromHtml(body));
            Assert.True(true);
        }

    }
}
