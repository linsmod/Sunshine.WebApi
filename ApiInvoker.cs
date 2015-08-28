using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApi
{
    /// <summary>
    /// Api调用客户端
    /// </summary>
    public class ApiInvoker
    {
        HttpClient client;

        /// <summary>
        /// 使用baseAddress构造Api调用客户端
        /// </summary>
        /// <param name="baseAddress"></param>
        public ApiInvoker(string baseAddress)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 获取BaseAddress
        /// </summary>
        public Uri BaseAddress { get; private set; }


        /// <summary>
        /// 执行Get获取单个或多个对象
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<TResponse> ExecuteGet<TResponse>(string url)
        {
            var ret = await client.GetAsync(url);
            var resp = await ret.Content.ReadAsAsync<ApiResponse<TResponse>>();
            return resp.data;
        }

        /// <summary>
        /// 执行Get获取支持翻页的列表
        /// </summary>
        /// <typeparam name="TListItem">元素类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public async Task<IApiPagedDataList<TListItem>> ExecutePagedGet<TListItem>(string url)
        {
            var ret = await client.GetAsync(url);
            var resp = await ret.Content.ReadAsAsync<ApiResponse<TListItem>>();
            return resp;
        }

        /// <summary>
        /// 执行带数据请求的Get
        /// </summary>
        /// <typeparam name="TResponse">返回数据类型</typeparam>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="req">提交的内容</param>
        /// <returns></returns>
        public async Task<TResponse> ExecuteGet<TResponse, TRequest>(string url, TRequest req)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<TRequest>(req, jsonFormatter);

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Content = content;

            var ret = await client.SendAsync(message, HttpCompletionOption.ResponseContentRead);
            var resp = await ret.Content.ReadAsAsync<ApiResponse<TResponse>>();
            return resp.data;
        }

        /// <summary>
        /// Get方式调用支持翻页的Api
        /// </summary>
        /// <typeparam name="TListItem">返回数据的元素类型</typeparam>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="req">提交的内容</param>
        /// <returns></returns>
        public async Task<IApiPagedDataList<TListItem>> ExecutePagedGet<TListItem, TRequest>(string url, TRequest req)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<TRequest>(req, jsonFormatter);

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Content = content;

            var ret = await client.SendAsync(message, HttpCompletionOption.ResponseContentRead);
            return await ret.Content.ReadAsAsync<ApiResponse<TListItem>>();
        }



        /// <summary>
        /// Post方式调用Api
        /// </summary>
        /// <typeparam name="TResponse">响应的数据类型</typeparam>
        /// <typeparam name="TRequest">提交的数据类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="req">提交的内容</param>
        /// <returns></returns>
        public async Task<TResponse> ExecutePost<TResponse, TRequest>(string url, TRequest req)
        {
            var ret = await client.PostAsJsonAsync(url, req);
            var resp = await ret.Content.ReadAsAsync<ApiResponse<TResponse>>();
            return resp.data;
        }

        /// <summary>
        /// Post方式调用后不获取结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task ExecutePost(string url)
        {
            await client.PostAsJsonAsync<object>(url, null);
        }

        /// <summary>
        /// Post方式调用支持翻页的Api
        /// </summary>
        /// <typeparam name="TListItem">返回数据的元素类型</typeparam>
        /// <typeparam name="TRequest">请求数据类型</typeparam>
        /// <param name="url">调用地址</param>
        /// <param name="req">提交的内容</param>
        /// <returns></returns>
        public async Task<IApiPagedDataList<TListItem>> ExecutePagedPost<TListItem, TRequest>(string url, TRequest req)
        {
            var ret = await client.PostAsJsonAsync(url, req);
            return await ret.Content.ReadAsAsync<ApiResponse<TListItem>>();
        }

        /// <summary>
        /// Post方式调用Api
        /// </summary>
        /// <typeparam name="TResponse">应答数据类型</typeparam>
        /// <param name="url">调用地址</param>
        /// <returns></returns>
        public async Task<TResponse> ExecutePost<TResponse>(string url)
        {
            var ret = await client.PostAsync(url, null);
            var resp = await ret.Content.ReadAsAsync<ApiResponse<TResponse>>();
            return resp.data;
        }

        /// <summary>
        /// Post方式调用无参的Api
        /// </summary>
        /// <typeparam name="TResponse">应答数据类型</typeparam>
        /// <param name="url">调用地址</param>
        /// <returns></returns>
        public async Task<IApiPagedDataList<TListItem>> ExecutePagedPost<TListItem>(string url)
        {
            var ret = await client.PostAsync(url, null);
            return await ret.Content.ReadAsAsync<ApiResponse<TListItem>>();
        }
    }
}
