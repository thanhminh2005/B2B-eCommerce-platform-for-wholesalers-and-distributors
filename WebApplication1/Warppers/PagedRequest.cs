using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Warppers
{
    public class PagedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PagedRequest()
        {
            PageNumber = 1;
            PageSize = 30;
        }

        public PagedRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
        }
    }
}
