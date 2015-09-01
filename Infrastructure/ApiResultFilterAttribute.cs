using Sunshine.WebApiLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Sunshine.WebApiLib.Infrastructure
{
    /// <summary>
    /// 默认Api结果处理筛选器
    /// </summary>
    public class DefaultApiResultFilterAttribute : ApiExecutionFilterAttribute
    {
        /// <summary>
        /// 核心处理器
        /// </summary>
        Sunshine.WebApiLib.Handlers.ApiExecutionContextHandler Handler { get; set; }
        public DefaultApiResultFilterAttribute()
        {
            Handler = new Sunshine.WebApiLib.Handlers.ApiExecutionContextHandler(this);
        }

        /// <summary>
        /// 处理执行前的验证错误
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            Handler.HandleExecutingContext(actionContext);
        }

        /// <summary>
        /// 处理和包装执行后的结果，包括异常（如果有的话）
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Handler.HandleExecutedContext(actionExecutedContext);
        }

        public override void InvokeBaseOnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void InvokeBaseOnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
