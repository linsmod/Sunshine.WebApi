using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using Sunshine.WebApiLib.Protocols;
using System.Web.Http;
namespace Sunshine.WebApiLib.Infrastructure.Authorization
{
    public abstract class ApiAuthorizationIdentityBinding<TUserId>
    {
        /// <summary>
        /// 关联一个认证特性，因为要获取角色信息和执行认证逻辑（通过OnAuthorization方法）
        /// </summary>
        protected AuthorizeAttribute AuthAttribute { get; set; }

        /// <summary>
        /// 用户认证主体信息预先绑定基类，用于实现将用户标识绑定到Http上下文及线程上下文的功能
        /// </summary>
        /// <param name="authAttribute"></param>
        public ApiAuthorizationIdentityBinding(AuthorizeAttribute authAttribute)
        {
            this.AuthAttribute = authAttribute;
        }

        /// <summary>
        /// 从当前Http(请求)上下文获取用户信息
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract TUserId GetUserId(HttpActionContext ctx);

        /// <summary>
        /// 判断用户标识是否有效
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected abstract bool IsUserIdValid(TUserId userId);

        /// <summary>
        /// 根据用户标识生成认证主体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected abstract IPrincipal GetPrincipalForUserId(TUserId userId);

        /// <summary>
        /// 将用户认证主体信息绑定到认证上下文
        /// </summary>
        /// <param name="ctx"></param>
        public void Bind(HttpActionContext ctx)
        {
            var userId = this.GetUserId(ctx);
            if (this.IsUserIdValid(userId))
            {
                var principal = this.GetPrincipalForUserId(userId);
                System.Threading.Thread.CurrentPrincipal = principal;
                System.Web.HttpContext.Current.User = principal;
            }
            else
            {
                //check if AllowAnonymousAttribute is set,
                //if set, should not response Unauthorized.
                var attr = ctx.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();
                bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);
                if (!isAnonymous)
                {
                    ctx.Response = ctx.Request.CreateResponse<ApiResult>(HttpStatusCode.OK, ApiResult.Unauthorized);
                }
            }
        }
    }
}
