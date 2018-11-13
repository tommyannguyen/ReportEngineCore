using DinkToPdf;
using System;

namespace Reporting
{
    public class Html2Pdf
    {
        public byte[] ExportFromHtml(string html)
        {
            return Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4Plus,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "", Line = false, Spacing = 2.812 }
                    }
                }
            });
        }
        public byte[] ExportFromUri(string uri)
        {
            return Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10 },
            },
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