using Sunshine.WebApiLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApiLib.Exceptions
{
    public class ApiResultException : Exception
    {
        public ApiResultException(int code, string msg, Exception innerException = null)
            : base(msg, innerException)
        {
            this.Result = new ApiResult(code, msg);
        }
        public ApiResultException(ApiResult result, Exception innerException = null)
            : base(result.msg, innerException)
        {
            this.Result = result;
        }
        public ApiResult Result { get; set; }
    }
}
