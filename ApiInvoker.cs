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
    public class ApiInvoker
    {
        HttpClient client;
        public ApiInvoker(string baseAddress)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Uri BaseAddress { get; set; }

        public async Task<TResponse> ExecuteGet<TResponse>(string url)
        {
            var ret = await client.GetAsync(url);
            var resp = await ret.Content.ReadAsAsync<TResponse>();
            return resp;
        }

        public async Task<TResponse> ExecutePost<TResponse>(string url)
        {
            var ret = await client.PostAsync(url, null);
            var resp = await ret.Content.ReadAsAsync<TResponse>();
            return resp;
        }

        public async Task<TResponse> ExecutePost<TResponse, TRequest>(string url, TRequest req)
        {
            //var product = new Product() { Name = "Gizmo", Price = 100, Category = "Widget" };

            //// Create the JSON formatter.
            //// 创建JSON格式化器。
            //MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

            //// Use the JSON formatter to create the content of the request body.
            //// 使用JSON格式化器创建请求体内容。
            //HttpContent content = new ObjectContent<Product>(product, jsonFormatter);

            //// Send the request.
            //// 发送请求。
            //var resp = client.PostAsync("api/products", content).Result;

            //上面是使用PostAsync的示例代码，这里使用PostAsJsonAsync扩展方法可以更简单
            var ret = await client.PostAsJsonAsync(url, req);
            var resp = await ret.Content.ReadAsAsync<TResponse>();
            return resp;
        }
    }
}
