using FitStop.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace FitStop.Api.Helpers
{
    /// <summary>
    /// Global exception handler that overrides the default one
    /// </summary>
    /// <seealso cref="System.Web.Http.ExceptionHandling.ExceptionHandler" />
    public class GlobalExceptionHandler : ExceptionHandler
    {

        /// <summary>
        /// When overridden in a derived class, handles the exception synchronously.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            Exception exception = context.Exception;

            HttpStatusCode statusCode;

            if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is RuntimeException)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            else if (exception is AuthenticationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            context.Result = new ResponseMessageResult(context.Request.CreateErrorResponse(statusCode, exception.Message));
        }
    }

}