using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Sunshine.WebApiLib.Infrastructure
{
    public abstract class ApiExecutionFilterAttribute : ActionFilterAttribute
    {
        public abstract void InvokeBaseOnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext);

        public abstract void InvokeBaseOnActionExecuted(HttpActionExecutedContext actionExecutedContext);
    }
}
