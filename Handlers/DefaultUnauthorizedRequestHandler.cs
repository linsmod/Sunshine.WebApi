using Sunshine.WebApiLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Sunshine.WebApiLib.Handlers
{
    public class DefaultUnauthorizedRequestHandler : IUnauthorizedRequestHandler
    {
        public ApiResult Handle(HttpActionContext ctx)
        {
            return new ApiResult(401, "UnauthorizedRequest.");
        }
    }
}
