using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Sunshine.WebApiLib.Infrastructure
{
    public class MvcActionFilterAttribute : ActionFilterAttribute
    {
        public void InvokeBaseOnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public void InvokeBaseOnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
