using FitStop.Api.Controllers;
using FitStop.Api.Models;
using FitStop.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FitStop.Api.Helpers
{
    /// <summary>
    /// Custom authorization attribute that uses JWT to authorize the user
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    public class TokenAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the roles the user is in.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public string Roles { get; set; }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <exception cref="AuthenticationException">
        /// No Authorization header present
        /// or
        /// Authorization header cannot be empty
        /// or
        /// Invalid token!
        /// or
        /// Token expired! Please, login again
        /// or
        /// You do not have permission to access this resource!
        /// </exception>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Skip validation if AllowAnonymous attribute is set
            if (!SkipValidation(actionContext))
            {
                // Check for authorization header
                var authorizationHeader = actionContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");
                if (authorizationHeader.Key == null)
                {
                    throw new AuthenticationException("No Authorization header present");
                }

                // Get token from Authorization header
                string tokenString = authorizationHeader.Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(tokenString))
                {
                    throw new AuthenticationException("Authorization header cannot be empty");
                }

                // Validate JWT token
                var secretKey = WebConfigurationManager.AppSettings["JwtSecret"];
                UserJwtModel user;

                try
                {
                    user = JWT.JsonWebToken.DecodeToObject<UserJwtModel>(tokenString, secretKey);
                }
                catch (JWT.SignatureVerificationException)
                {
                    throw new AuthenticationException("Invalid token!");
                }

                if (user.ExpirationDate < DateTime.UtcNow)
                {
                    throw new AuthenticationException("Token expired! Please, login again");
                }

                // Validate roles
                if (Roles != null && !Roles.Split(',').ToList().Contains(user.Role))
                {
                    throw new AuthenticationException("You do not have permission to access this resource!");
                }

                // Add current user to base controller
                var controller = actionContext.ControllerContext.Controller as BaseController;
                controller.CurrentUser = user;
            }

            base.OnActionExecuting(actionContext);
        }

        private bool SkipValidation(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }

}