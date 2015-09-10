using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Webdiyer.WebControls.Mvc;

namespace Sunshine.WebApiLib.Protocols
{
    /// <summary>
    /// 表示一般返回结果
    /// </summary>
    public interface IApiResult
    {
        int StatusCode { get; }
        string Message { get; }
    }

    /// <summary>
    /// 表示返回的数据
    /// </summary>
    public interface IApiData
    {
        object Data { get; }
    }

    /// <summary>
    /// 表示数据列表
    /// </summary>
    public interface IApiDataList<T>
    {
        IEnumerable<T> Items { get; }
    }

    public interface IApiPagedDataList<T> : IApiDataList<T>
    {
        bool HasMore { get; }
    }

    /// <summary>
    /// 仅结果
    /// </summary>
    public class ApiResult : IApiResult
    {
        public ApiResult() { }
        public ApiResult(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
        /// <summary>
        /// 响应码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string msg { get; set; }

        int IApiResult.StatusCode
        {
            get { return this.code; }
        }

        string IApiResult.Message
        {
            get { return this.msg; }
        }

        /// <summary>
        /// 0 for success request
        /// </summary>
        public static ApiResult Success = new ApiResult();

        /// <summary>
        /// 401 for unauthorized request
        /// </summary>
        public static ApiResult Unauthorized = new ApiResult(401, "Unauthorized");
    }

    /// <summary>
    /// 结果+数据
    /// </summary>
    public class ApiResultWithData : ApiResult, IApiData
    {
        public object data { get; set; }

        object IApiData.Data
        {
            get { return this.data; }
        }
    }

    /// <summary>
    /// 结果+列表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultWithDataList<T> : ApiResultWithData, IApiDataList<T>
    {
        IEnumerable<T> IApiDataList<T>.Items
        {
            get { return (this.data as IEnumerable).Cast<T>(); }
        }
    }

    /// <summary>
    /// 结果+列表数据+翻页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultWithPagedDataList<T> : ApiResultWithData, IApiPagedDataList<T>
    {
        public bool hasMore { get; set; }

        bool IApiPagedDataList<T>.HasMore
        {
            get { return this.hasMore; }
        }

        IEnumerable<T> IApiDataList<T>.Items
        {
            get { return (this.data as IEnumerable).Cast<T>(); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponse<T> : IApiPagedDataList<T>, IApiDataList<T>, IApiData, IApiResult
    {
        public ApiResponse(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public ApiResponse()
        {

        }
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

        /// <summary>
        /// 列表是否有更多数据
        /// </summary>
        public bool hasMore { get; set; }

        object IApiData.Data
        {
            get { return this.data; }
        }

        int IApiResult.StatusCode
        {
            get { return this.code; }
        }

        string IApiResult.Message
        {
            get { return this.msg; }
        }



        bool IApiPagedDataList<T>.HasMore
        {
            get { return this.hasMore; }
        }

        IEnumerable<T> IApiDataList<T>.Items
        {
            get { return (this.data as IEnumerable).Cast<T>(); }
        }
    }


    public class ApiResponse : ApiResponse<object>
    {

        public ApiResponse() { }
    }
}
