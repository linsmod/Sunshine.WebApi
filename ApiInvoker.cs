using Sunshine.WebApiLib.Protocols;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sunshine.WebApiLib
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
            client.DefaultRequestHeaders.Accept.Clear();
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
            var ret = client.GetAsync(url).Result;
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
            var ret = client.GetAsync(url).Result;
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

            var ret = client.SendAsync(message, HttpCompletionOption.ResponseContentRead).Result;
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

            var ret = client.SendAsync(message, HttpCompletionOption.ResponseContentRead).Result;
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
            var ret = client.PostAsJsonAsync(url, req).Result;
            var resp = await ret.Content.ReadAsAsync<ApiResponse<TResponse>>();
            return resp.data;
        }

        /// <summary>
        /// Post方式调用后获取结果,不获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ApiResult> ExecutePost(string url)
        {
            var resp = client.PostAsync(url, null).Result;
            return await resp.Content.ReadAsAsync<ApiResult>();
        }

        /// <summary>
        /// Post方式调用后获取结果，不获取数据
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ApiResult> ExecutePost<TRequest>(string url, TRequest req)
        {
            var ret = client.PostAsJsonAsync(url, req).Result;
            ApiResult result;
            if (TryHandleException(ret, out result))
            {
                return result;
            }
            return await ret.Content.ReadAsAsync<ApiResult>();
        }

        private bool TryHandleException(HttpResponseMessage resp, out ApiResult result)
        {
            if (resp.IsSuccessStatusCode)
            {
                result = null;
                return false;
            }
            var msg = resp.Content.ReadAsStringAsync().Result;
            result = new ApiResult(500, msg);
            return true;
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
            var ret = client.PostAsJsonAsync(url, req).Result;
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
            var ret = client.PostAsync(url, null).Result;
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
            var ret = client.PostAsync(url, null).Result;
            return await ret.Content.ReadAsAsync<ApiResponse<TListItem>>();
        }
    }
}