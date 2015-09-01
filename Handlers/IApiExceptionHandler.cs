using Sunshine.WebApiLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApiLib.Handlers
{
    public interface IApiExceptionHandler
    {
        ApiResult Handle(Exception exception);
    }
}
