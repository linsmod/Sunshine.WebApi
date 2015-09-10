using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sunshine.WebApiLib.Infrastructure.Authorization
{
    /// <summary>
    /// 用户认证信息预绑定默认实现
    /// </summary>
    public class DefaultMvcUserAuthorizeAttribute : AuthorizeAttribute
    {
        DefaultMvcUserIdBinding UserBinding { get; set; }

        /// <summary>
        /// 用户认证信息预绑定默认实现
        /// </summary>
        public DefaultMvcUserAuthorizeAttribute()
        {
            UserBinding = new DefaultMvcUserIdBinding(this);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            UserBinding.Bind(filterContext);
        }
    }
}
