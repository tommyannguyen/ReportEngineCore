# ReportEngineCore
.Net core report engine
For reporting 

 var html2Pdf = new Html2Pdf();
            var outPutDirectory = AppDomain.CurrentDomain.BaseDirectory;

            IHtmlRenderEngine viewEngine = new FluidHtmlViewEngine();
            IHtmlRenderEngine preMailerEngine = new HtmlPreMailerViewEngine(outPutDirectory);
            var model = new { Name = "Con bướm xinh", Job = 100 };
            var htmlBody = await File.ReadAllTextAsync($"Views\\index.html");

            var body = await viewEngine.RenderAsync(model, htmlBody);
            body = await preMailerEngine.RenderAsync(model, body);

            File.WriteAllBytes("tententen.html", Encoding.UTF8.GetBytes(body));
            File.WriteAllBytes("tententen.pdf", html2Pdf.ExportFromHtml(body));
            Assert.True(true);