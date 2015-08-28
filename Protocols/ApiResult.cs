using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace Sunshine.WebApi.Protocols
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


    public class ApiResponse : ApiResponse<object> {
        public ApiResponse(int code, string msg) : base(code, msg) { }
        public ApiResponse() { }
    }
}
