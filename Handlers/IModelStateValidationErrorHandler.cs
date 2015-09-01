using Sunshine.WebApiLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Sunshine.WebApiLib.Handlers
{
    public interface IModelStateValidationErrorHandler
    {
        ApiResult Handle(ModelStateDictionary modelState);
    }
}
