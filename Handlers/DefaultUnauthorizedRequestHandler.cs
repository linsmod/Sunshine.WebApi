using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Sunshine.WebApi.Handlers
{
    public class DefaultUnauthorizedRequestHandler : IUnauthorizedRequestHandler
    {
        public ApiResponse Handle(HttpActionContext ctx)
        {
            return new ApiResponse(401, "UnauthorizedRequest.");
        }
    }
}
