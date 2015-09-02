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
    }
}
