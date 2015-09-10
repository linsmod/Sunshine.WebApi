using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sunshine.WebApiLib.Infrastructure.Authorization
{
    /// <summary>
    /// 用户Id绑定器默认实现
    /// </summary>
    public class DefaultMvcUserIdBinding : MvcAuthorizationIdentityBinding<int>
    {
        /// <summary>
        /// 用户Id绑定器默认实现
        /// </summary>
        /// <param name="authAttribute">关联的用于预先认证的特性实例</param>
        public DefaultMvcUserIdBinding(AuthorizeAttribute authAttribute)
            : base(authAttribute)
        {

        }

        protected override int GetUserId(HttpContextBase ctx)
        {
            try
            {
                int userId;
                string session = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                var ticket = FormsAuthentication.Decrypt(session);
                if (!ticket.Expired && int.TryParse(ticket.Name, out userId))
                {
                    return userId;
                }
            }
            catch { }
            return -1;
        }

        /// <summary>
        /// 简单的判断用户Id是否大于0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected override bool IsUserIdValid(int userId)
        {
            return userId > 0;
        }

        protected override System.Security.Principal.IPrincipal GetPrincipalForUserId(int userId)
        {
            var identity = new GenericIdentity(userId.ToString(), "ApplicationCookie");
            return new GenericPrincipal(identity, this.AuthAttribute.Roles.Split(','));
        }
    }
}
