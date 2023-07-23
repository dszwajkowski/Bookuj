using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Common.Models
{
    public class PaginatedList<T>
    {             
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }

        public PaginatedList(List<T> data, int pageNumber, int pageSize, int count)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;            
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PageCount;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, pageNumber, pageSize, count);
        }

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, pageNumber, pageSize, count);
        }
    }
}
