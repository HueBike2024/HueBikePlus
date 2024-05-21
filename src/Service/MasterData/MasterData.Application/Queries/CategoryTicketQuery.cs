using Azure.Core;
using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using MasterData.Application.Commands.CategoryTicketCommand;
using MasterData.Application.Commands.LocationCommand;
using MasterData.Application.DTOs.CategoryTicket;
using MasterData.Application.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface ICategoryTicketQuery
    {
        /// <summary>
        /// Danh sách loại vé
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<CategoryticketResponse>> ListAsync(ListCategoryTicketCommand command);
    }

    public class CategoryTicketQuery : ICategoryTicketQuery
    {
        private readonly IRepository<CategoryTicket> _cateRep;
        public CategoryTicketQuery(IRepository<CategoryTicket> cateRep)
        {
            _cateRep = cateRep;
        }
        public async Task<PagingResultSP<CategoryticketResponse>> ListAsync(ListCategoryTicketCommand command)
        {
            var query = _cateRep.GetQuery();

            if (!string.IsNullOrEmpty(command.SearchTerm))
            {
                command.SearchTerm = command.SearchTerm.ToLower().Trim();
                // Thử chuyển đổi SearchTerm sang long
                long searchTermAsLong;
                bool isNumeric = long.TryParse(command.SearchTerm, out searchTermAsLong);
                command.SearchTerm = command.SearchTerm.ToLower().Trim();
                query = query.Where(e =>
                    e.CategoryTicketName.ToLower().Contains(command.SearchTerm) ||
                    e.Id == searchTermAsLong || // So sánh với ID dạng long
                    (isNumeric && e.Id == searchTermAsLong)
                );// Kiểm tra nếu SearchTerm có thể chuyển thành long
            }
            var categoryTicketResponse = query.Select(e => new CategoryticketResponse
            {
                Id = e.Id,
                CategoryTicketName = e.CategoryTicketName,
                Description = e.Description,
                Price = e.Price,
                UserTime = e.UserTime,
                CreatedDate = e.CreatedDate,
                UpdatedDate = e.UpdatedDate

            });

            if (string.IsNullOrEmpty(command.OrderBy) && string.IsNullOrEmpty(command.OrderByDesc))
            {
                categoryTicketResponse = categoryTicketResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                categoryTicketResponse = PagingSorting.Sorting(command, categoryTicketResponse);
            }
            var pageIndex = command.PageSize * (command.PageIndex - 1);

            var response = await PaginatedList<CategoryticketResponse>.CreateAsync(categoryTicketResponse, pageIndex, command.PageSize);
            var result = new PagingResultSP<CategoryticketResponse>(response, response.Total, command.PageIndex, command.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }
    }
}
