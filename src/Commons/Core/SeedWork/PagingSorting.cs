using Core.Extensions;
using System.Linq;

namespace Core.SeedWork
{
    public static class PagingSorting
    {
        public static IQueryable<T> Sorting<T>(PagingQuery searchRequest, IQueryable<T> query)
        {
            if (searchRequest == null)
            {
                return query;
            }
            if (!string.IsNullOrEmpty(searchRequest.OrderBy) && searchRequest.GetFieldMapping().ContainsKey(searchRequest.OrderBy.ToLower()))
            {
                string sortField = searchRequest.GetFieldMapping()[searchRequest.OrderBy.ToLower()];
                query = query.OrderBy(sortField);
            }
            else if (!string.IsNullOrEmpty(searchRequest.OrderByDesc) && searchRequest.GetFieldMapping().ContainsKey(searchRequest.OrderByDesc.ToLower()))
            {
                string sortField = searchRequest.GetFieldMapping()[searchRequest.OrderByDesc.ToLower()];
                query = query.OrderByDescending(sortField);
            }

            return query;
        }
    }
}
