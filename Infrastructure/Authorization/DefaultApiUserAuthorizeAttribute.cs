using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Sunshine.WebApiLib.Infrastructure.Authorization
{
    /// <summary>
    /// 用户认证信息预绑定默认实现
    /// </summary>
    public class DefaultApiUserAuthorizeAttribute : ApiAuthorizeAttribute
    {
        DefaultApiUserIdBinding UserBinding { get; set; }

        /// <summary>
        /// 用户认证信息预绑定默认实现
        /// </summary>
        public DefaultApiUserAuthorizeAttribute()
        {
            UserBinding = new DefaultApiUserIdBinding(this);
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            UserBinding.Bind(actionContext);
        }
    }
}
