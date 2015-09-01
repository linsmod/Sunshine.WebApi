using Sunshine.WebApiLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace Sunshine.WebApiLib.Extensions
{
    public static class ApiResponseExtension
    {
        public static ApiResponse AsApiResponse(this IPagedList source)
        {
            var totalPage = GetTotalPageCount(source.TotalItemCount, source.PageSize);
            return new ApiResponse()
            {
                data = source.Cast<object>(),
                hasMore = totalPage - source.CurrentPageIndex > 0
            };
        }

        private static int GetTotalPageCount(int totalItemCount, int pageSize)
        {
            return totalItemCount == 0 ? 0 : (int)Math.Ceiling((double)(((double)totalItemCount) / ((double)pageSize)));
        }
    }
}
