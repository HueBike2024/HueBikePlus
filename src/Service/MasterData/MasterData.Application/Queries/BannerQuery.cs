using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BannerAggregate;
using MasterData.Application.Commands.BannerCommand;
using MasterData.Application.DTOs.Banner;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Application.Queries
{
    public interface IBannerQuery
    {
        /// <summary>
        /// Chi tiết thông tin banner
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<BannerDetailResponse> GetAsync(GetBannerCommand command);
        /// <summary>
        /// Danh sách bai viet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<BannerResponse>> ListAsync(ListBannerCommand command);
    }
    public class BannerQuery : IBannerQuery
    {
        private readonly IRepository<Banner> _banRep;
        public BannerQuery(IRepository<Banner> banRep)
        {
            _banRep = banRep;
        }
        public async Task<BannerDetailResponse> GetAsync(GetBannerCommand request)
        {
            return await _banRep.GetQuery(e => e.Id == request.Id)

               .Select(k => new BannerDetailResponse
               {
                   Id = k.Id,
                   Tilte = k.Title,
                   Image = k.Image,
                   Type = k.Type,
                   CreateDate = DateTime.Now,


               }).FirstOrDefaultAsync();
        }

        // Danh sách bannner

        public async Task<PagingResultSP<BannerResponse>> ListAsync(ListBannerCommand request)
        {
            var query = _banRep.GetQuery();

            // Filter by Type
            if (!string.IsNullOrEmpty(request.Type))
            {
                query = query.Where(e => e.Type == request.Type);
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e => e.Title.ToLower().Contains(request.SearchTerm));
            }

            var banResponse = query.Select(e => new BannerResponse
            {
                Id = e.Id,
                Tilte = e.Title,
                Image = e.Image,
                Type = e.Type,
                CreatedDate = e.CreatedDate,
            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                banResponse = banResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                banResponse = PagingSorting.Sorting(request, banResponse);
            }

            var pageIndex = request.PageSize * (request.PageIndex - 1);
            var response = await PaginatedList<BannerResponse>.CreateAsync(banResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<BannerResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;
        }

    }
}
