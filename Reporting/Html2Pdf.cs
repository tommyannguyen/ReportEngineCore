using DinkToPdf;
using System;

namespace Reporting
{
    public sealed class Html2Pdf : IHtml2Pdf
    {
        private readonly GlobalSettings _globalSettings;
        public Html2Pdf(GlobalSettings globalSettings = null)
        {
            _globalSettings = globalSettings ?? new GlobalSettings()
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10 },
            };
        }
        public byte[] ExportFromHtml(string html)
        {
            return Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = _globalSettings,
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { Spacing = 2.812 }
                    }
                }
            });
        }
        public byte[] ExportFromUri(string uri)
        {
            return Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = _globalSettings,
                Objects = {
                new ObjectSettings()
                {
                    Page = uri,
                },
                }
            });
        }

        private byte[] Convert(HtmlToPdfDocument doc)
        {
            return new SynchronizedConverter(new PdfTools()).Convert(doc);
        }
    }
}