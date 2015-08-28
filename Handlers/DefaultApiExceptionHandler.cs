using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApi.Handlers
{
    /// <summary>
    /// 默认Api异常处理器
    /// </summary>
    public class DefaultApiExceptionHandler : IApiExceptionHandler
    {
        public ApiResponse Handle(Exception exception)
        {
            return new ApiResponse(500, exception.Message);
        }
    }
}
