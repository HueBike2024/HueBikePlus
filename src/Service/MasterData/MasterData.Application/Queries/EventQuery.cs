using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.EventAggregate;
using MasterData.Application.Commands.EventCommand;
using MasterData.Application.DTOs.Event;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Application.Queries
{
    public interface IEventQuery
    {
        /// <summary>
        /// Chi tiết thông tin sự kiện
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<EventDetailResponse> GetAsync(GetEventCommand command);
        /// <summary>
        /// Danh sách sự kiện
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<EventResponse>> ListAsync(ListEventCommand command);
    }
    public class EventQuery : IEventQuery
    {
        private readonly IRepository<Event> _evtRep;
        public EventQuery(IRepository<Event> evtRep)
        {
            _evtRep = evtRep;
        }
        public async Task<EventDetailResponse> GetAsync(GetEventCommand request)
        {
            return await _evtRep.GetQuery(e => e.Id == request.Id)

               .Select(k => new EventDetailResponse
               {
                   Id = k.Id,
                   Title = k.Title,
                   Image = k.Image,
                   Description = k.Description,
                   DateStart = k.DateStart,
                   DateEnd = k.DateEnd,
                   Location = k.Location,
                   Organizer = k.Organizer,
                   
                   CreateDate = DateTime.Now,


               }).FirstOrDefaultAsync();
        }

        // Danh sách bai viet

        public async Task<PagingResultSP<EventResponse>> ListAsync(ListEventCommand request)
        {
            var query = _evtRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e => e.Title.ToLower().Contains(request.SearchTerm));

            }
            var evtResponse = query.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Image = e.Image,
                Description = e.Description,
                DateStart = e.DateStart,
                DateEnd = e.DateEnd,
                Location = e.Location,
                Organizer = e.Organizer,
               

                CreateDate = e.CreatedDate,


            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                evtResponse = evtResponse.OrderByDescending(e => e.CreateDate);
            }
            else
            {
                evtResponse = PagingSorting.Sorting(request, evtResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<EventResponse>.CreateAsync(evtResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<EventResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }

    }
}
