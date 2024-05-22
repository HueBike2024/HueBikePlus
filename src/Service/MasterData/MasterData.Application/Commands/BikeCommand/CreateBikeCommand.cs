using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Core.Utility;
using GoogleApi.Entities.Maps.Common;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using ZXing.Common;
using ZXing;
using Bike = Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate.Bike;
using Infrastructure.AggregatesModel.MasterData.ImageAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using MasterData.Application.Services.CloudinaryService;

namespace MasterData.Application.Commands.BikeCommand
{
    public class CreateBikeCommand : IRequest<bool>
    {
        public long? BikeId { get; set; }
        public int? Power { get; set; }
        public long? StationId { get; set; }
        public long StatusId { get; set; }
        public string BaseQrUrl { get; set; }
    }
    public class CreateBikeCommandHandler : IRequestHandler<CreateBikeCommand, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudPhotoService _cloudService;
        public readonly IRepository<Bike> _bikeRep;
        public readonly IRepository<Status> _statusRep;
        public readonly IRepository<Station> _stationRep;
        public readonly IRandomService _randomService;


        public readonly IUnitOfWork _unitOfWork;


        public CreateBikeCommandHandler(ICloudPhotoService cloudService, IHttpContextAccessor httpContextAccessor,IRepository<Bike> bikeRep, IUnitOfWork unitOfWork, IRepository<Station> stationRep, IRepository<Status> statusRep, IRandomService randomService)
        {
            _cloudService = cloudService;
            _bikeRep = bikeRep;
            _unitOfWork = unitOfWork;
            _stationRep = stationRep;
            _httpContextAccessor = httpContextAccessor;
            _statusRep = statusRep;
            _randomService = randomService;
        }
        public async Task<bool> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Method == HttpMethods.Post && (request.BikeId == null || request.BikeId == 0))
            {

                var status = await _statusRep.FindOneAsync(e => e.Id == request.StatusId);
                // Kiểm tra trạng thái
                if (status == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Trạng thái");
                }

                //var isExistQr = await _bikeRep.GetAny(e => e.PathQr == request.LockId && e.IsUsed == true);

                //// xét trường hợp khóa đã đc dùng hay chưa
                //if (isExisLock)
                //{
                //    throw new BaseException(ErrorsMessage.MSG_EXIST, "Khóa");
                //}


                var bike = new Bike("", request.StatusId, request.StationId);

                if (request.StationId != null || request.StationId != 0)
                {
                    var station = await _stationRep.FindOneAsync(e => e.Id == request.StationId);

                    // Kiểm tra số lượng chỗ trong trạm
                    if (station.QuantityAvaiable == station.NumOfSeats)
                    {
                        throw new BaseException("Số chỗ của trạm đã đạt giới hạn, vui lòng chọn trạm khác");
                    }

                    station.QuantityAvaiable += 1;
                    _stationRep.Update(station);
                }
                _bikeRep.Add(bike);

                await _unitOfWork.SaveChangesAsync();

                //tạo mã Qr và gàn lại cho bike
                var pathQr = $"{request.BaseQrUrl}/master-data/api/trip/new-trip?Id={bike.Id}";

                //kiểm tra Qr
                if (pathQr != null)
                {
                    var isExistQr = await _bikeRep.GetAny(e => e.PathQr == pathQr && e.Id != request.BikeId);

                    // xét trường hợp khóa đã đc dùng hay chưa
                    if (isExistQr)
                    {
                        throw new BaseException(ErrorsMessage.MSG_EXIST, "Khóa");
                    }

                    if (request.Power == null)
                    {
                        throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Thời lượng pin");
                    }

                    var barcodeWriter = new BarcodeWriterPixelData
                    {
                        Format = BarcodeFormat.QR_CODE,
                        Options = new EncodingOptions
                        {
                            Height = 200,
                            Width = 200
                        }
                    };

                    var pixelData = barcodeWriter.Write(pathQr);

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
                        string base64QrCode = Convert.ToBase64String(qrImage); // Chuyển đổi sang base64
                        if (base64QrCode != null && base64QrCode.Length > 0)
                        {

                            var uploadResult = await _cloudService.UploadPhotoFromBase64Async(base64QrCode, "QrCode");

                            if (uploadResult != null)
                            {
                                bike.QrCodeImage = uploadResult.Url;
                            }
                            else
                            {
                                throw new BaseException("Không thể tải lên hình ảnh!");
                            }
                        }
                    }

                    bike.PathQr = pathQr;
                    bike.Power = request.Power;
                }

                if (request.Power != null && pathQr == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "PathQr");
                }

                //Tạo mã cho xe( tên xe)
                var bikeCode = "Bike" + bike.Id;

                bike.BikeCode = bikeCode;
                _bikeRep.Update(bike);

                await _unitOfWork.SaveChangesAsync();

                return true;

            }
            else
            {
                var bike = await _bikeRep.FindOneAsync(e => e.Id == request.BikeId);
                if (bike == null)
                {
                    throw new BaseException("Không tìm thấy xe!");
                }

                var station = await _stationRep.FindOneAsync(e => e.Id == request.StationId);
                // Kiểm tra số lượng chỗ trong trạm
                if (station.QuantityAvaiable == station.NumOfSeats)
                {
                    throw new BaseException("Số chỗ của trạm đã đạt giới hạn, vui lòng chọn trạm khác");
                }
                var stationold = await _stationRep.FindOneAsync(e => e.Id == bike.StationId);
               

                var status = await _statusRep.FindOneAsync(e => e.Id == request.StatusId);
                // Kiểm tra trạng thái
                if (status == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Trạng thái");
                }



                bike.Power = request.Power;
                bike.StatusId = request.StatusId;
                bike.StationId = request.StationId;

                if (stationold.Id != station.Id)
                {
                    stationold.QuantityAvaiable--;
                    station.QuantityAvaiable++;
                }

                _bikeRep.Update(bike);
                station.QuantityAvaiable += 1;
                _stationRep.Update(station);

                await _unitOfWork.SaveChangesAsync();

                return true;


            }


        }

    }

}
