using Sunshine.WebApiLib.Exceptions;
using Sunshine.WebApiLib.Protocols;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sunshine.WebApiLib
{
    /// <summary>
    /// Api调用客户端
    /// </summary>
    public abstract class ApiInvoker
    {
        HttpClient client;
        HttpClientHandler clientHandler;
        CookieContainer cookieContainer;

        /// <summary>
        /// 使用baseAddress构造Api调用客户端
        /// </summary>
        /// <param name="baseAddress"></param>
        public ApiInvoker(string baseAddress)
        {
            cookieContainer = new CookieContainer();
            clientHandler = new HttpClientHandler { UseCookies = true, CookieContainer = cookieContainer };
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
             where TResponse : class
        {
            BindSession();
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
            where TListItem : class
        {
            BindSession();
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
            where TRequest : class
            where TResponse : class
        {
            BindSession();
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
            where TRequest : class
            where TListItem : class
        {
            BindSession();
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
            where TRequest : class
            where TResponse : class
        {
            BindSession();
            var ret = client.PostAsJsonAsync(url, req).Result;
            if (!ret.IsSuccessStatusCode)
            {
                var x = ret.Content.ReadAsStringAsync().Result;
                throw new ApiResultException((int)ret.StatusCode, x);
            }

            var resp = await ret.Content.ReadAsAsync<ApiResponse<TResponse>>();
            if (resp.code != 0)
                ThrowApiResultException((IApiResult)resp);
            return resp.data;
        }


        /// <summary>
        /// Post方式调用后获取结果,不获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ApiResult> ExecutePost(string url)
        {
            BindSession();
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
            where TRequest : class
        {
            BindSession();
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

        private void ThrowApiResultException(IApiResult result)
        {
            if (result.StatusCode != 0)
                throw new Exceptions.ApiResultException(result.StatusCode, result.Message);
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
            where TRequest : class
            where TListItem : class
        {
            BindSession();
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
            where TResponse : class
        {
            BindSession();
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
            where TListItem : class
        {
            BindSession();
            var ret = client.PostAsync(url, null).Result;
            return await ret.Content.ReadAsAsync<ApiResponse<TListItem>>();
        }

        /// <summary>
        /// 设置用户会话
        /// </summary>
        /// <param name="session">会话ID，如果为null将在客户端移除会话ID</param>
        public void SetSession(string session)
        {
            if (session == null)
            {
                client.DefaultRequestHeaders.Authorization = null;
            }
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Session", session);
        }


        /// <summary>
        /// 绑定会话Id
        /// </summary>
        private void BindSession()
        {
            var sid = this.GetSessionId();
            this.SetSession(sid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetSessionId();
    }
}