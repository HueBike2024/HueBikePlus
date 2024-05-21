using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.AuthenticationAggregate;
using MediatR;
using Infrastructure.Services;
using MasterData.Application.Services.OcrService;
using MasterData.Application.Services.TextParserService;
using MasterData.Application.DTOs.UserAuthentication;
using Core.Infrastructure.Handlers;

namespace MasterData.Application.Commands.UserAuthenticationCommand
{
    public class AddAuthenInfoCommand : IRequest<AddAuthenInfoResponse>
    {
        public string CardId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DayOfBirth { get; set; }
    }

    public class AddAuthenInfoCommandHandle : BaseHandler, IRequestHandler<AddAuthenInfoCommand, AddAuthenInfoResponse>
    {
        private readonly IRepository<User> _userRep;
        private readonly IRepository<UserAuthentication> _authenRep;
        private readonly IUnitOfWork _unitOfWork;

        public AddAuthenInfoCommandHandle(IRepository<User> userRep, IRepository<UserAuthentication> authenRep, IUnitOfWork unitOfWork)
        {
            _userRep = userRep;
            _authenRep = authenRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddAuthenInfoResponse> Handle(AddAuthenInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRep.FindOneAsync(e => e.Id == UserId);

            if (user == null)
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Người dùng");
            }

            var userAuthen = await _authenRep.FindOneAsync(e => e.CardId == request.CardId);

            if (userAuthen != null)
            {
                throw new BaseException("Số thẻ đã tồn tại hoặc không hợp lệ!");
            }

            var userCheck = await _authenRep.FindOneAsync(e => e.UserId == UserId);
            if (userCheck != null)
            {
                throw new BaseException("Bạn đã xác thực thông tin trước đó rồi!");
            }

            if (request.CardId.Length != 12 || !request.CardId.All(char.IsDigit))
            {
                throw new BaseException("Số thẻ phải chứa chính xác 12 chữ số!");
            }

            if (request.IssueDate == default(DateTime))
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Ngày cấp");
            }

            if (request.ExpiryDate == default(DateTime))
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Ngày hết hạn");
            }

            if (request.DayOfBirth == default(DateTime))
            {
                throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Ngày sinh");
            }
            //DateTime issueDate = default;
            //DateTime expiryDate = default;

            ////quét hình ảnh sang dữ liệu thô
            //string ocrFrontImageText = _ocrService.PerformOCR(request.FrontImage);
            //string ocrBackImageText = _ocrService.PerformOCR(request.BackImage);

            ////Chuyển đổi dãy kí tự thành các trường và giá trị
            //var frontImageData = _authenService.ParseTextFrontOfCard(ocrFrontImageText);
            //var backImageData = _authenService.ParseTextFrontOfCard(ocrBackImageText);

            ////Truyền dữ liệu từ data về dữ liệu authen model
            //var frontData = frontImageData.Select(kvp => new { FieldName = kvp.Key, Value = kvp.Value }).ToList();
            //var backData = backImageData.Select(kvp => new { FieldName = kvp.Key, Value = kvp.Value }).ToList();

            ////Thêm hình ảnh vào authmedia
            //var frontImg = await _fileService.UploadStreamFile(request.FrontImage, user.FullName, "", false, 300, false);
            //var backImg = await _fileService.UploadStreamFile(request.BackImage, user.FullName, "", false, 300, false);

            //if(frontImg == null)
            //{
            //    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Mặt trước thẻ");
            //}
            //if (backImg == null)
            //{
            //    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Mặt sau thẻ");
            //}

            //if (frontData.Count > 0 && backData.Count > 0)
            //{
            //    string pattern = @"(\d{1,2})/(\d{1,2})/(\d{4})";

            //    Match imatch = Regex.Match(backData[0].Value, pattern);
            //    if (imatch.Success)
            //    {
            //        // Lấy giá trị của ngày, tháng và năm từ kết quả khớp
            //        string iday = imatch.Groups[1].Value;
            //        string imonth = imatch.Groups[2].Value;
            //        string iyear = imatch.Groups[3].Value;
            //        issueDate = DateTime.ParseExact($"{iday}/{imonth}/{iyear}", "d/M/yyyy", CultureInfo.InvariantCulture);
            //    }
            //    Match ematch = Regex.Match(frontData[7].Value, pattern);
            //    if (ematch.Success)
            //    {
            //        // Lấy giá trị của ngày, tháng và năm từ kết quả khớp
            //        string eday = ematch.Groups[1].Value;
            //        string emonth = ematch.Groups[2].Value;
            //        string eyear = ematch.Groups[3].Value;
            //        expiryDate = DateTime.ParseExact($"{eday}/{emonth}/{eyear}", "d/M/yyyy", CultureInfo.InvariantCulture);
            //    }
            //}

            var userAuthentication = new UserAuthentication(request.CardId, request.IssueDate, request.ExpiryDate, UserId);
            var result = new AddAuthenInfoResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                CardId = request.CardId,
                IssueDate = request.IssueDate,
                ExpiryDate = request.ExpiryDate,
                DayOfBirth = request.DayOfBirth,
            };

            _authenRep.Add(userAuthentication);
            await _unitOfWork.SaveChangesAsync();

            user.AuthenId = userAuthentication.Id;
            user.IsConfirm = true;
            user.DateOfBirth = request.DayOfBirth;

            _userRep.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Trả về user authentication result
            return result;
        }

    }
}
