using Sunshine.WebApiLib.Exceptions;
using Sunshine.WebApiLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sunshine.WebApiLib.Extensions;
using Sunshine.WebApiLib.Protocols;
namespace Sunshine.WebApiLib.Handlers
{
    /// <summary>
    /// 执行上下文处理器
    /// </summary>
    public class MvcHttpContextHandler : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 执行上下文处理器
        /// </summary>
        /// <param name="onUnauthorized"></param>
        public MvcHttpContextHandler()
        {
        }
        protected HttpContextBase HttpContext { get; private set; }

        public void HandleExecutingContext(ActionExecutingContext ctx)
        {
            this.HttpContext = ctx.HttpContext;
            var modelState = ((Controller)ctx.Controller).ModelState;
            ActionResult result;
            //如果有模型校验错误就返回
            if (!modelState.IsValid && this.HandleModelStateValidationError(modelState, out result))
            {
                ctx.Result = result;
                return;
            }
            else
                base.OnActionExecuting(ctx);
        }

        public void HandleExecutedContext(ActionExecutedContext ctx)
        {
            this.HttpContext = ctx.HttpContext;
            var modelState = ((Controller)ctx.Controller).ModelState;
            ActionResult resp;
            //如果有模型校验错误就返回
            if (!modelState.IsValid && this.HandleModelStateValidationError(modelState, out resp))
            {
                ctx.Result = resp;
                return;
            }

            base.OnActionExecuted(ctx);
            if (ctx.Exception != null && this.HandleException(ctx.Exception, out resp))
            {
                ctx.Result = resp;
                ctx.ExceptionHandled = true;
                return;
            }
        }
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool HandleException(Exception exception, out ActionResult result)
        {

            if (exception is ApiResultException)
            {
                var apiRet = ((ApiResultException)exception).Result;
                if (apiRet.code == 401)
                {
                    result = new RedirectResult("http://i.play7th.com/account/login?returnUrl=" + this.HttpContext.Request.Url.ToString());
                    return true;
                }
                if (this.HttpContext.Request.IsAjaxRequest())
                {
                    result = new JsonResult() { Data = (((ApiResultException)exception).Result), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    return true;
                }
            }
            else if (exception is System.AggregateException)
            {
                exception = exception.InnerException;
                return this.HandleException(exception, out result);
            }
            else if (exception is EfException)
            {
                if (this.HttpContext.Request.IsAjaxRequest())
                {
                    result = new JsonResult() { Data = new ApiResult(7001, exception.Message), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
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
        private bool HandleModelStateValidationError(ModelStateDictionary modelState, out ActionResult result)
        {
            var msg = modelState.GetFirstError();
            if (this.HttpContext.Request.IsAjaxRequest())
            {
                result = new JsonResult()
                {
                    Data = new ApiResult(200, msg),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return true;
            }
            result = null;
            return false;
        }

        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            this.HttpContext = filterContext.HttpContext;
            ActionResult result;
            if (this.HandleException(filterContext.Exception, out result))
            {
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
