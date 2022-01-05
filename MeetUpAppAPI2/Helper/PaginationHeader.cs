using Microsoft.AspNetCore.Http;
using System.Text.Json;
namespace MeetUpAppAPI.Helper
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int CurrentPage {get;set;}
        public int ItemsPerPage {get;set;}
        public int TotalItems {get;set;}
        public int TotalPages {get;set;}
    }

    public static class HttpExtensions {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
                                                int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

    }
}