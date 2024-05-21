using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.PostAggregate;
using MasterData.Application.Commands.PostCommand;
using MasterData.Application.DTOs.Post;
using Microsoft.EntityFrameworkCore;

namespace MasterData.Application.Queries
{
    public interface IPostQuery
    {
        /// <summary>
        /// Chi tiết thông tin baif vieets
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<PostDetailResponse> GetAsync(GetPostCommand command);
        /// <summary>
        /// Danh sách bai viet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<PostResponse>> ListAsync(ListPostCommand command);
    }
    public class PostQuery : IPostQuery
    {
        private readonly IRepository<Post> _postRep;
        public PostQuery(IRepository<Post> postRep)
        {
            _postRep = postRep;
        }
        public async Task<PostDetailResponse> GetAsync(GetPostCommand request)
        {
            return await _postRep.GetQuery(e => e.Id == request.Id)

               .Select(k => new PostDetailResponse
               {
                   Id = k.Id,
                   Tilte = k.Title,
                   Image = k.Image,
                   Content = k.Content,
                   CreateDate = DateTime.Now,
                   

               }).FirstOrDefaultAsync();
        }

        // Danh sách bai viet

        public async Task<PagingResultSP<PostResponse>> ListAsync(ListPostCommand request)
        {
            var query = _postRep.GetQuery();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                query = query.Where(e => e.Title.ToLower().Contains(request.SearchTerm));

            }
            var postResponse = query.Select(e => new PostResponse
            {
                Id = e.Id,
                Tilte = e.Title,
                Content = e.Content,
                Image = e.Image,
                CreatedDate = e.CreatedDate,


            });

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                postResponse = postResponse.OrderByDescending(e => e.CreatedDate);
            }
            else
            {
                postResponse = PagingSorting.Sorting(request, postResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<PostResponse>.CreateAsync(postResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<PostResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }

    }
}
