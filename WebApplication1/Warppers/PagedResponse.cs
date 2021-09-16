using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Warppers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            Succeeded = true;
            Errors = null;
            Message = null;
            Data = data;
        }

        public PagedResponse(T data, int pageNumber, int pageSize, int total)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            Total = total;
            Succeeded = true;
            Errors = null;
            Message = null;
            Data = data;
        }
    }
}
