using System.Collections.Generic;

namespace ProductsApp.Domain.DTOs
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public PaginatedResponse()
        {
        }

        public PaginatedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (totalRecords + pageSize - 1) / pageSize; // Ceiling division
            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;
        }
    }
}
