using DinkToPdf;
using DinkToPdf.Contracts;
using Core.Interfaces.Pdf;

namespace Core.Implements.Pdf
{
    public class PdfService : IPdfService
    {
        private readonly IConverter _pdfConverter;

        public PdfService(IConverter converter)
        {
            _pdfConverter = converter;
        }

        public byte[] Convert(string htmlContent, PechkinPaperSize paperSize)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = paperSize,
                        Margins = new MarginSettings { Top = 10 },
                    },
                Objects = {
                        new ObjectSettings() {
                            HtmlContent = htmlContent,
                            WebSettings = { DefaultEncoding = "utf-8" },
                            HeaderSettings = { FontName = "Times New Roman", FontSize = 12, Right = "[page]/[toPage]", Line = false, Spacing = 2.812},
                            FooterSettings = { FontName = "Times New Roman", FontSize = 12, Center = "", Line = false, Spacing = 2.812 }
                        }
                    }
            };

            return _pdfConverter.Convert(doc);
        }
    }
}
