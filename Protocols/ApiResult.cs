using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace Sunshine.WebApi.Protocols
{
    public interface IApiResult
    {
        int code { get; set; }
        string msg { get; set; }
    }
    /// <summary>
    /// 表示Api应答结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : IApiResult
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public T data { get; set; }
    }

    /// <summary>
    /// 表示Api应答结果
    /// </summary>
    public class ApiResult : ApiResult<Object>, IApiResult
    {
        public ApiResult() { }
        public ApiResult(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
    }

    /// <summary>
    /// 带翻页的Api应答结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedApiResult<T> : ApiResult<IEnumerable<T>>, IApiResult
    {
        /// <summary>
        /// 是否还有更多数据
        /// </summary>
        public bool hasMore { get; set; }
    }

    public class PagedApiResult : PagedApiResult<object>, IApiResult
    {
        public static PagedApiResult FromPagedList(IPagedList source)
        {
            var totalPage = GetTotalPageCount(source.TotalItemCount, source.PageSize);
            return new PagedApiResult
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
