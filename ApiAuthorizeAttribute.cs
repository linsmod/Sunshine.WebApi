using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Properties;
namespace Sunshine.WebApiLib
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private static readonly string[] _emptyArray = new string[0];
        private readonly object _typeId = new object();
        private string _roles;
        private string[] _rolesSplit = ApiAuthorizeAttribute._emptyArray;
        private string _users;
        private string[] _usersSplit = ApiAuthorizeAttribute._emptyArray;
        /// <summary>Gets or sets the authorized roles. </summary>
        /// <returns>The roles string. </returns>
        public string Roles
        {
            get
            {
                return this._roles ?? string.Empty;
            }
            set
            {
                this._roles = value;
                this._rolesSplit = ApiAuthorizeAttribute.SplitString(value);
            }
        }
        /// <summary>Gets a unique identifier for this attribute.</summary>
        /// <returns>A unique identifier for this attribute.</returns>
        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }
        /// <summary>Gets or sets the authorized users. </summary>
        /// <returns>The users string. </returns>
        public string Users
        {
            get
            {
                return this._users ?? string.Empty;
            }
            set
            {
                this._users = value;
                this._usersSplit = ApiAuthorizeAttribute.SplitString(value);
            }
        }
        /// <summary>Indicates whether the specified control is authorized.</summary>
        /// <returns>true if the control is authorized; otherwise, false.</returns>
        /// <param name="actionContext">The context.</param>
        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;
            return principal != null 
                && principal.Identity != null 
                && principal.Identity.IsAuthenticated 
                && (this._usersSplit.Length <= 0 || this._usersSplit.Contains(principal.Identity.Name, StringComparer.OrdinalIgnoreCase)) 
                && (this._rolesSplit.Length <= 0 || this._rolesSplit.Any(new Func<string, bool>(principal.IsInRole)));
        }
        /// <summary>Calls when an action is being authorized.</summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="T:System.ArgumentNullException">The context parameter is null.</exception>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            if (ApiAuthorizeAttribute.SkipAuthorization(actionContext))
            {
                return;
            }
            if (!this.IsAuthorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
        }
        /// <summary>Processes requests that fail authorization.</summary>
        /// <param name="actionContext">The context.</param>
        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized Request");
        }
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>() || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>();
        }
        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return ApiAuthorizeAttribute._emptyArray;
            }
            IEnumerable<string> source =
                from piece in original.Split(new char[]
                {
                    ','
                })
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;
            return source.ToArray<string>();
        }
    }
}
