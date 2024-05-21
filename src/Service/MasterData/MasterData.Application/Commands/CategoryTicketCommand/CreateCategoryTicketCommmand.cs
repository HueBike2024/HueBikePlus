using Core.Exceptions;
using Core.Interfaces.Database;
using Core.Properties;
using Core.SeedWork.Repository;
using GoogleApi.Entities.Interfaces;
using Infrastructure.AggregatesModel.Authen.AccountAggregate;
using Infrastructure.AggregatesModel.Authen;
using Infrastructure.AggregatesModel.MasterData.UserAggregate.ComplainAggregate;
using MasterData.Application.Commands.ComplainCommand;
using MasterData.Application.DTOs.CategoryTicket;
using MasterData.Application.DTOs.Complain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using MasterData.Application.DTOs.Status;
using Microsoft.AspNetCore.Http;
using Core.Helpers.Cryptography;

namespace MasterData.Application.Commands.CategoryTicketCommand
{
    public class CreateCategoryTicketCommmand : IRequest<CategoryticketResponse>
    {
        public long? CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double UserTime { get; set; }
    }

    public class CreateCategoryTicketCommmandHandler : IRequestHandler<CreateCategoryTicketCommmand, CategoryticketResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IRepository<CategoryTicket> _cateRep;
        public readonly IRepository<User> _userRep;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AuthenMedia> _media;

        public CreateCategoryTicketCommmandHandler(IHttpContextAccessor httpContextAccessor, IRepository<CategoryTicket> cateRep, IUnitOfWork unitOfWork, IRepository<AuthenMedia> media, IRepository<User> userRep)
        {
            _httpContextAccessor = httpContextAccessor;
            _cateRep = cateRep;
            _userRep = userRep;
            _unitOfWork = unitOfWork;
            _media = media;
        }
        public async Task<CategoryticketResponse> Handle(CreateCategoryTicketCommmand request, CancellationToken cancellationToken)
        {
            // Lấy đối tượng HttpContext từ IHttpContextAccessor
            var httpContext = _httpContextAccessor.HttpContext;
            // Kiểm tra giao thức HTTP
            if (httpContext.Request.Method == HttpMethods.Post && (request.CategoryId == null || request.CategoryId == 0))
            {
                var isEXName = await _cateRep.GetAny(e => e.CategoryTicketName == request.Name);

                if (isEXName)
                {
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên loại vé");
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Tên loại vé");
                }

                if (request.Price < 0)
                {
                    throw new BaseException("Giá loại vé không hợp lệ");
                }

                if (request.Price == 0)
                {
                    throw new BaseException("Giá loại vé không được để trống");
                }

                if (request.UserTime == null)
                {
                    throw new BaseException("Thời gian sử dụng không được để trống");
                }

                var categoryTicket = new CategoryTicket(request.Name, request.Description, request.Price, request.UserTime);

                _cateRep.Add(categoryTicket);

                await _unitOfWork.SaveChangesAsync();


                var result = new CategoryticketResponse
                {
                    Id = categoryTicket.Id,
                    CategoryTicketName = categoryTicket.CategoryTicketName,
                    Description = categoryTicket.Description,
                    Price = categoryTicket.Price,
                    UserTime = categoryTicket.UserTime,
                    CreatedDate = categoryTicket.CreatedDate,
                };

                return result;
            }
            else
            {
                var categoryTicket = await _cateRep.FindOneAsync(e => e.Id == request.CategoryId);
                if (categoryTicket == null)
                {
                    throw new BaseException("Không tìm thấy loại vé này!");
                }

                var isEXName = await _cateRep.GetAny(e => e.CategoryTicketName == request.Name);

                if (isEXName)
                {
                    throw new BaseException(ErrorsMessage.MSG_EXIST, "Tên loại vé");
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new BaseException(ErrorsMessage.MSG_NOT_EXIST, "Tên loại vé");
                }

                if (request.Price < 0)
                {
                    throw new BaseException("Giá loại vé không hợp lệ");
                }

                if (request.Price == 0)
                {
                    throw new BaseException("Giá loại vé không được để trống");
                }

                if (request.UserTime == null)
                {
                    throw new BaseException("Thời gian sử dụng không được để trống");
                }

                categoryTicket.CategoryTicketName = request.Name;
                categoryTicket.Description = request.Description;
                categoryTicket.Price = request.Price;
                categoryTicket.UserTime = request.UserTime;
                categoryTicket.UpdatedDate = DateTime.Now;

                _cateRep.Update(categoryTicket);

                await _unitOfWork.SaveChangesAsync();


                var result = new CategoryticketResponse
                {
                    Id = categoryTicket.Id,
                    CategoryTicketName = categoryTicket.CategoryTicketName,
                    Description = categoryTicket.Description,
                    Price = categoryTicket.Price,
                    UserTime = categoryTicket.UserTime,
                    CreatedDate = categoryTicket.CreatedDate,
                    UpdatedDate = categoryTicket.UpdatedDate,
                };

                return result;
            }
        }
    }
}
