using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Net.Http;
using System.Net;
using Webdiyer.WebControls.Mvc;
namespace Sunshine.WebApi.Handlers
{
    /// <summary>
    /// 执行上下文处理器
    /// </summary>
    public class ApiExecutionContextHandler
    {
        /// <summary>
        /// 关联的过滤器
        /// </summary>
        public ActionFilterAttribute AssociateFilter { get; set; }

        /// <summary>
        /// 未认证的处理
        /// </summary>
        public IUnauthorizedRequestHandler UnauthorizedRequestHandler { get; set; }

        /// <summary>
        /// 异常处理
        /// </summary>
        public IApiExceptionHandler ExceptionHandler { get; set; }
        /// <summary>
        /// 模型验证错误处理
        /// </summary>
        public IModelStateValidationErrorHandler ModelStateValidationErrorHandler { get; set; }

        /// <summary>
        /// 构造ApiExecutionContextHandler
        /// </summary>
        /// <param name="attr"></param>
        public ApiExecutionContextHandler(ActionFilterAttribute attr)
        {
            ExceptionHandler = new DefaultApiExceptionHandler();
            ModelStateValidationErrorHandler = new DefaultModelStateValidationErrorHandler();
            UnauthorizedRequestHandler = new DefaultUnauthorizedRequestHandler();
            this.AssociateFilter = attr;
        }
        /// <summary>
        /// MVC模型验证检查
        /// </summary>
        /// <param name="actionContext"></param>
        public void HandleExecutingContext(HttpActionContext ctx)
        {
            IApiResult result;
            if (!ctx.ModelState.IsValid && this.HandleModelStateValidationError(ctx.ModelState, out result))
            {
                ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.OK, result);
                return;
            }
            AssociateFilter.OnActionExecuting(ctx);
        }

        public void HandleExecutedContext(HttpActionExecutedContext ctx)
        {
            IApiResult result = new ApiResult();
            //如果有模型校验错误就返回
            if (!ctx.ActionContext.ModelState.IsValid && this.HandleModelStateValidationError(ctx.ActionContext.ModelState, out result))
            {
                ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.OK, result);
                return;
            }

            AssociateFilter.OnActionExecuted(ctx);
            if (ctx.Exception != null && this.HandleException(ctx.Exception, out result))
            {
                ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                if (ctx.Response.StatusCode == HttpStatusCode.Unauthorized && this.HandleUnauthorizedRequest(ctx.ActionContext, out result))
                {
                    return;
                }
                else if (ctx.Response.StatusCode != HttpStatusCode.OK && ctx.Response.StatusCode != HttpStatusCode.NoContent)
                {
                    result = new ApiResult((int)ctx.Response.StatusCode, ctx.Response.StatusCode.ToString());
                }
                else
                {
                    var cnt = ctx.ActionContext.Response.Content;
                    if (cnt != null)
                    {
                        var objCnt = (ObjectContent)cnt;
                        if (typeof(IPagedList).IsAssignableFrom(objCnt.ObjectType))
                        {
                            result = PagedApiResult.FromPagedList((IPagedList)objCnt.Value);
                        }
                        else
                            ((ApiResult)result).data = ctx.ActionContext.Response.Content.ReadAsAsync<object>().Result;
                    }
                }
            }
            ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool HandleException(Exception exception, out IApiResult result)
        {

            if (this.ExceptionHandler != null)
            {
                result = this.ExceptionHandler.Handle(exception);
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// 处理模型校验错误
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool HandleModelStateValidationError(ModelStateDictionary modelState, out IApiResult result)
        {
            if (this.ModelStateValidationErrorHandler != null)
            {
                result = this.ModelStateValidationErrorHandler.Handle(modelState);
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// 处理未登录的请求
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool HandleUnauthorizedRequest(HttpActionContext ctx, out IApiResult result)
        {
            if (this.UnauthorizedRequestHandler != null)
            {
                result = this.UnauthorizedRequestHandler.Handle(ctx);
                return true;
            }
            result = null;
            return false;
        }
    }
}
