using Sunshine.WebApi.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Sunshine.WebApi.Handlers
{
    /// <summary>
    /// 模型校验错误默认处理器
    /// </summary>
    public class DefaultModelStateValidationErrorHandler : IModelStateValidationErrorHandler
    {
        public virtual ApiResult Handle(ModelStateDictionary modelState)
        {
            var msg = RetrieveErrors(modelState);
            return new ApiResult(7001, msg);
        }

        /// <summary>
        /// 收集校验错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        protected virtual string RetrieveErrors(ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, string>();
            foreach (KeyValuePair<string, ModelState> keyValue in modelState)
            {
                errors[keyValue.Key] = string.Join("; ", keyValue.Value.Errors.Select(e => (e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message)));
            }
            var errorString = string.Join("; ", errors.Select(x => string.Format("属性: \"{0}\", 错误信息: \"{1}\"", x.Key, x.Value)));
            return errorString;
        }
    }
}
