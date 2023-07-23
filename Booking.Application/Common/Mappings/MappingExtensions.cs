using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
        {
            return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
        }

        public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable, int pageNumber, int pageSize)
        {
            return PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);
        }

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
        {
            return queryable.ProjectTo<TDestination>(configuration).ToListAsync();
        }
    }
}
