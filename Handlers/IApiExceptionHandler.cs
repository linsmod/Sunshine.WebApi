using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApi.Handlers
{
    public interface IApiExceptionHandler
    {
        ApiResponse Handle(Exception exception);
    }
}
