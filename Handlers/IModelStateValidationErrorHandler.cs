using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Sunshine.WebApi.Handlers
{
    public interface IModelStateValidationErrorHandler
    {
        ApiResponse Handle(ModelStateDictionary modelState);
    }
}
