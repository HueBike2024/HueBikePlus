using Core.Interfaces.Database;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using MediatR;
using ZXing.Common;
using ZXing;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Core.Exceptions;
using Core.Properties;

namespace MasterData.Application.Commands.BikeLockCommand
{
    public class CreateLockCommand : IRequest<long>
    {
        public string LockName { get; set; }
        public string PathQr { get; set; }
      
        public int Power { get; set; }
        public bool IsUsed { get; set; }
    }
    public class CreateLockCommandHandler : IRequestHandler<CreateLockCommand, long>
    {
        public readonly IRepository<BikeLock> _lockRep;
        public readonly IUnitOfWork _unitOfWork;


        public CreateLockCommandHandler(IRepository<BikeLock> lockRep, IUnitOfWork unitOfWork)
        {
            _lockRep = lockRep;
            _unitOfWork = unitOfWork;
        }
        public async Task<long> Handle(CreateLockCommand request, CancellationToken cancellationToken)
        {
            

            // Tạo đối tượng BikeLock từ request
            var newLock = new BikeLock
            {
                LockName = request.LockName,
                PathQr = request.PathQr,
                Power = request.Power,
                IsUsed = request.IsUsed,
            };
            // Tạo mã QR Code từ đường dẫn PathQr
            var barcodeWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 200, // Điều chỉnh kích thước theo nhu cầu
                    Width = 200   // Điều chỉnh kích thước theo nhu cầu
                }
            };

            var pixelData = barcodeWriter.Write(request.PathQr);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                        pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var qrImage = ms.ToArray();
                newLock.QrCodeImage = qrImage; // Lưu ảnh QR Code vào BikeLock
            }

          

            var isExistName = await _lockRep.GetAny(e => e.LockName.Trim().ToLower() == request.LockName.Trim().ToLower());

            if (isExistName)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "Name");
            }
            var isExisPathQR = await _lockRep.GetAny(e => e.PathQr.Trim().ToLower() == request.PathQr.Trim().ToLower());

            if (isExisPathQR)
            {
                throw new BaseException(ErrorsMessage.MSG_EXIST, "PathQr");
            }


            // Thêm đối tượng mới vào repository và lưu thay đổi
            _lockRep.Add(newLock);
            await _unitOfWork.SaveChangesAsync();

            return newLock.Id; // Trả về ID của khoá vừa được tạo
        }






    }
}
