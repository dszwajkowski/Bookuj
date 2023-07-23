using Booking.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#nullable disable

namespace Booking.WebUI.Pages.Shared
{
    public class _PaginatedListPartialModel : PageModel
    {
        [BindProperty]
        public int PageSize { get; set; } = 25;

        public dynamic PaginatedList { get; set; }
        public string Handler { get; set; }
        public string EmptyListErrorMessage { get; set; }

        public static _PaginatedListPartialModel Create<T>(PaginatedList<T> paginatedList, string handler = null
            , string emptyListErrorMessage = "Lista jest pusta") where T : class, new()
        {
            return new _PaginatedListPartialModel
            {
                Handler = handler,
                PaginatedList = paginatedList,
                EmptyListErrorMessage = emptyListErrorMessage
            };
        }
    }
}
