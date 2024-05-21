using Core.Exceptions;
using Core.Infrastructure.Handlers;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.ExtendEntities;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using Infrastructure.AggregatesModel.MasterData.UserAggregate;
using Infrastructure.Services;
using MasterData.Application.Commands.TransactionCommmand;
using MasterData.Application.DTOs.Ticket;
using MasterData.Application.DTOs.Transaction;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing;
using MasterData.Application.Services.CloudinaryService;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;

namespace MasterData.Application.Commands.TicketCommand
{
    public class PreBookTicketCommand  : IRequest<TicketResponse>
    {
        public long? TicketId { get; set; }
        public string? UserPhone { get; set; }
        public long? BikeId { get; set; }
        public long CategoryTicketId { get; set; }
        public DateTime BookingDate { get; set; }
    }

    public class PreBookTicketCommandHandler : BaseHandler, IRequestHandler<PreBookTicketCommand, TicketResponse>
    {
        public readonly IRepository<Ticket> _tickRep;
        public readonly IRepository<Transaction> _tranRep;
        public readonly IRepository<Bike> _bikeRep;
        public readonly IRepository<CategoryTicket> _cateRep;
        public readonly IRepository<User> _userRep;
        public readonly IRepository<Status> _statusRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRandomService _randomService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudPhotoService _cloudService;

        public PreBookTicketCommandHandler(IRepository<Status> statusRep, ICloudPhotoService cloudService, IRepository<Ticket> tickRep, IRandomService randomService, IRepository<Bike> bikeRep, IRepository<User> userRep, IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork, IRepository<Transaction> tranRep, IHttpContextAccessor httpContextAccessor)
        {
            _tickRep = tickRep;
            _bikeRep = bikeRep;
            _cateRep = cateRep;
            _userRep = userRep;
            _statusRep = statusRep;
            _unitOfWork = unitOfWork;
            _tranRep = tranRep;
            _randomService = randomService;
            _cloudService = cloudService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TicketResponse> Handle(PreBookTicketCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);
            var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == request.CategoryTicketId);
            var bike = await _bikeRep.FindOneAsync(e => e.Id == request.BikeId);
            var tickets = await _tickRep.GetQuery().Where(e => e.BikeId == request.BikeId && e.BookingDate.Date == request.BookingDate.Date).ToListAsync();

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Method == HttpMethods.Post && (request.TicketId == null || request.TicketId == 0))
            {
                if ((user.IsSuperAdmin == true && request.UserPhone == null) || (user == null))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
                }

                if (user.IsSuperAdmin == false && request.UserPhone != null && user.PhoneNumber != request.UserPhone)
                {
                    throw new BaseException("Số điện thoại của bạn không khớp!");
                }

                if (user.IsSuperAdmin == true && request.UserPhone != null)
                {
                    user = await _userRep.FindOneAsync(e => e.PhoneNumber == request.UserPhone);
                }

                if (bike == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Xe");
                }

                if (categoryTicket == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Loại Vé");
                }

                if (request.BookingDate == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Ngày đặt");
                }

                if (request.BookingDate < DateTime.Now.Date)
                {
                    throw new BaseException("Ngày đặt không đúng!");
                }
                if (request.BookingDate.Date == DateTime.Now.Date && request.BookingDate.TimeOfDay < DateTime.Now.TimeOfDay)
                {
                    throw new BaseException("Giờ đặt không đúng");
                }

                if (user.IsSuperAdmin == false && user.Point < categoryTicket.Price)
                {
                    throw new BaseException("Người dùng không đủ điểm để mua loại vé này!");
                }

                //Kiểm tra thời gian đặt vé
                if (tickets != null)
                {
                    foreach (var ticket in tickets)
                    {
                        var cateTicket = await _cateRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);
                        DateTime expectedEndTime = ticket.BookingDate.AddHours(categoryTicket.UserTime);
                        var isBooked = await _tickRep.GetAny(e => e.BikeId == request.BikeId && e.BookingDate <= request.BookingDate && request.BookingDate <= expectedEndTime);

                        if (isBooked)
                        {
                            throw new BaseException("Xe đã được đặt trước vào thời điểm này!");
                        }
                    }
                }

                if (request.UserPhone != null)
                {
                    user = await _userRep.FindOneAsync(e => e.PhoneNumber == request.UserPhone);
                }

                //Tạo mã vé
                var ticketNo = _randomService.GenerateRandomString(16);
                var isTicketNo = await _tickRep.GetAny(e => e.TicketNo == ticketNo);
                while (isTicketNo)
                {
                    ticketNo = _randomService.GenerateRandomString(16);
                }

                //Kiểm tra status
                var status = await _statusRep.FindOneAsync(e => e.StatusName.Trim().ToLower() == "Chưa sử dụng".Trim().ToLower());
                if (status == null)
                {
                    status = new Status("Chưa sử dụng");
                    _statusRep.Add(status);
                    await _unitOfWork.SaveChangesAsync();
                }

                //Tạo vé chưa có Qrcode
                var ticketadd = new Ticket(ticketNo, user.FullName, user.PhoneNumber, "", "", request.BookingDate, request.CategoryTicketId, user.Id, request.BikeId, (decimal)categoryTicket.Price, status.Id);


                //Tạo Qrcode, upload và lấy về img
                var pathQr = _randomService.GenerateRandomString(32);
                var isPathQr = await _tickRep.GetAny(e => e.PathQr == pathQr);
                while (isPathQr)
                {
                    pathQr = _randomService.GenerateRandomString(32);
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
                            ticketadd.PathQr = pathQr;
                            ticketadd.QrImage = uploadResult.Url;
                        }
                        else
                        {
                            throw new BaseException("Không thể tải lên hình ảnh!");
                        }
                    }
                }

                //Thời gian kết thúc dự kiến và thời gian hết hạn
                if (categoryTicket.CategoryTicketName.Trim().ToLower().Contains("Vé giờ".Trim().ToLower()) || categoryTicket.CategoryTicketName.Trim().ToLower().Contains("Vé Ngày".Trim().ToLower()))
                {
                    ticketadd.ExpectedEndTime = ticketadd.BookingDate.AddHours(categoryTicket.UserTime);
                    ticketadd.ExpiryDate = ticketadd.BookingDate.AddHours(categoryTicket.UserTime).AddDays(1);
                }
                else
                {
                    if (categoryTicket.CategoryTicketName.Trim().ToLower().Contains("Vé tuần".Trim().ToLower()))
                    {
                        ticketadd.ExpiryDate = ticketadd.BookingDate.AddDays(8);
                    }
                    else
                    {
                        ticketadd.ExpiryDate = ticketadd.BookingDate.AddDays(31);
                    }
                }

                //Trừ điểm của người dùng
                user.Point -= categoryTicket.Price;

                _tickRep.Add(ticketadd);
                _userRep.Update(user);

                var transaction = new Transaction("Mua vé", _randomService.GenerateRandomString(16), -categoryTicket.Price, user.Id, true);
                _tranRep.Add(transaction);


                await _unitOfWork.SaveChangesAsync();

                var ticketResponse = new TicketResponse
                {
                    Id = ticketadd.Id,
                    TicketNo = ticketadd.TicketNo,
                    UserId = user.Id,
                    UserFullName = user.FullName,
                    UserPhone = user.PhoneNumber,
                    BookingDate = ticketadd.BookingDate,
                    ExpectedEndTime = ticketadd.ExpectedEndTime,
                    ExpiryDate = ticketadd.ExpiryDate,
                    QrImg = ticketadd.QrImage,
                    BikeId = ticketadd.BikeId,
                    BikeName = bike.BikeName,
                    CategoryTicketId = categoryTicket.Id,
                    CategoryTicketName = categoryTicket.CategoryTicketName,
                    Price = ticketadd.Price,
                    StatusId = status.Id,
                    Status = status.StatusName,
                };

                return ticketResponse;
            }
            else
            {
                var ticketUpdate = await _tickRep.FindOneAsync(e => e.Id == request.TicketId);
                var curCategoryTicket = await _cateRep.FindOneAsync(e => e.Id == ticketUpdate.CategoryTicketId);
                var status = await _statusRep.FindOneAsync(e => e.Id ==ticketUpdate.StatusId);

                if ((user.IsSuperAdmin == true && request.UserPhone == null) || (user == null))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
                }

                if (user.IsSuperAdmin == false && request.UserPhone != null && user.PhoneNumber != request.UserPhone)
                {
                    throw new BaseException("Số điện thoại của bạn không khớp!");
                }

                if (user.IsSuperAdmin == true && request.UserPhone != null)
                {
                    user = await _userRep.FindOneAsync(e => e.PhoneNumber == request.UserPhone);
                }

                if (ticketUpdate.BookingDate <= DateTime.Now)
                {
                    throw new BaseException("Vé đã quá giờ đặt cũ, không thể thay đổi lịch đặt!");
                }

                if (bike == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Xe");
                }

                if (categoryTicket == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Loại Vé");
                }

                if (request.BookingDate == null)
                {
                    throw new BaseException(ErrorsMessage.MSG_REQUIRED, "Ngày đặt");
                }

                if (request.BookingDate < DateTime.Now.Date)
                {
                    throw new BaseException("Ngày đặt không đúng!");
                }
                if (request.BookingDate.Date == DateTime.Now.Date && request.BookingDate.TimeOfDay < DateTime.Now.TimeOfDay)
                {
                    throw new BaseException("Giờ đặt không đúng");
                }

                if (ticketUpdate.CategoryTicketId != request.CategoryTicketId)
                {
                    user.Point = user.Point + curCategoryTicket.Price;
                }

                if (user.Point < categoryTicket.Price)
                {
                    throw new BaseException("Bạn không đủ điểm để mua loại vé này!");
                }

                //Kiểm tra thời gian đặt vé
                if (tickets != null)
                {
                    foreach (var ticket in tickets)
                    {
                        var cateTicket = await _cateRep.FindOneAsync(e => e.Id == ticket.CategoryTicketId);
                        DateTime expectedEndTime = ticket.BookingDate.AddHours(categoryTicket.UserTime);
                        var isBooked = await _tickRep.GetAny(e => e.BikeId == request.BikeId && e.BookingDate <= request.BookingDate && request.BookingDate <= expectedEndTime);

                        if (isBooked)
                        {
                            throw new BaseException("Xe đã được đặt trước vào thời điểm này!");
                        }
                    }
                }

                ticketUpdate.BikeId = request.BikeId;
                ticketUpdate.CategoryTicketId = request.CategoryTicketId;
                ticketUpdate.BookingDate = request.BookingDate;
                ticketUpdate.UpdatedDate = DateTime.UtcNow;

                //Thời gian kết thúc dự kiến và thời gian hết hạn
                if (categoryTicket.CategoryTicketName.Trim().ToLower().Contains("Vé giờ".Trim().ToLower()) || categoryTicket.CategoryTicketName.Trim().ToLower().Contains("Vé Ngày".Trim().ToLower()))
                {
                    ticketUpdate.ExpectedEndTime = ticketUpdate.BookingDate.AddHours(categoryTicket.UserTime);
                    ticketUpdate.ExpiryDate = ticketUpdate.BookingDate.AddHours(categoryTicket.UserTime).AddDays(1);
                }
                else
                {
                    if (categoryTicket.CategoryTicketName.Trim().ToLower().Contains("Vé tuần".Trim().ToLower()))
                    {
                        ticketUpdate.ExpiryDate = ticketUpdate.BookingDate.AddDays(8);
                    }
                    else
                    {
                        ticketUpdate.ExpiryDate = ticketUpdate.BookingDate.AddDays(31);
                    }
                }

                ticketUpdate.Price = categoryTicket.Price;
                user.Point -= categoryTicket.Price;

                var priceChange = categoryTicket.Price - curCategoryTicket.Price;
                if (priceChange != 0)
                {
                    var transaction = new Transaction("Thay đổi thông tin đặt vé", _randomService.GenerateRandomString(16), priceChange, user.Id, true);
                    _tranRep.Add(transaction);
                }

                _tickRep.Update(ticketUpdate);
                _userRep.Update(user);

                await _unitOfWork.SaveChangesAsync();

                var ticketResponse = new TicketResponse
                {
                    Id = ticketUpdate.Id,
                    TicketNo = ticketUpdate.TicketNo,
                    UserId = user.Id,
                    UserFullName = ticketUpdate.UserFullName,
                    UserPhone = ticketUpdate.UserPhone,
                    BookingDate = ticketUpdate.BookingDate,
                    ExpectedEndTime = ticketUpdate.ExpectedEndTime,
                    ExpiryDate = ticketUpdate.ExpiryDate,
                    QrImg = ticketUpdate.QrImage,
                    BikeId = ticketUpdate.BikeId,
                    BikeName = bike.BikeName,
                    CategoryTicketId = categoryTicket.Id,
                    CategoryTicketName = categoryTicket.CategoryTicketName,
                    Price = ticketUpdate.Price,
                    StatusId = ticketUpdate.StatusId,
                    Status = status.StatusName,
                };

                return ticketResponse;
            }
        }
    }
}
