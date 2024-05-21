using DinkToPdf;

namespace Core.Interfaces.Pdf
{
    public interface IPdfService
    {
        byte[] Convert(string htmlContent, PechkinPaperSize paperSize);
    }
}
