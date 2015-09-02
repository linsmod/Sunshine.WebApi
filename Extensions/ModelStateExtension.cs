using Sunshine.WebApiLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sunshine.WebApiLib.Extensions
{
    public static class ModelStateExtension
    {
        public static string GetFirstError(this ModelStateDictionary modelState)
        {
            return modelState.Values.Where(x => x.Errors.Any()).First().Errors[0].ErrorMessage;
        }

        public static string GetFirstError(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            return modelState.Values.Where(x => x.Errors.Any()).First().Errors[0].ErrorMessage;
        }

        public static ApiResult AsApiResult(this System.Web.Http.ModelBinding.ModelStateDictionary modelState, int code = 7001)
        {
            return new ApiResult(code, modelState.GetFirstError());
        }

        public static ApiResult AsApiResult(this ModelStateDictionary modelState, int code = 7001)
        {
            return new ApiResult(code, modelState.GetFirstError());
        }
    }
}
