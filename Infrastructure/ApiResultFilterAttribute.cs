using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Sunshine.WebApi.Infrastructure
{
    public class DefaultApiResultFilterAttribute : ActionFilterAttribute
    {
        Sunshine.WebApi.Handlers.ApiExecutionContextHandler Handler { get; set; }
        public DefaultApiResultFilterAttribute()
        {
            Handler = new Sunshine.WebApi.Handlers.ApiExecutionContextHandler(this);
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            Handler.HandleExecutingContext(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Handler.HandleExecutedContext(actionExecutedContext);
        }
    }
}
