using System.IO;
using Tesseract;

namespace MasterData.Application.Services.OcrService
{
    public class OCRService
    {
        public string PerformOCR(Stream imageStream)
        {
            // Đọc dữ liệu từ luồng hình ảnh và lưu vào một mảng byte
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);
                imageBytes = ms.ToArray();
            }

            // Tạo hình ảnh từ mảng byte
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (var engine = new TesseractEngine("./tessdata", "vie", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromMemory(imageBytes))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
        }
    }
}
