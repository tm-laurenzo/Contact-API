using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class PaginationMetadata
    {
        public PaginationMetadata(int totalCount, int currentPage, int itemsPerPage)
        {
           
        }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;    
    }
}
