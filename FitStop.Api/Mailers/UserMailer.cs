using FitStop.Entities;
using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitStop.Api.Mailers
{
    /// <summary>
    /// User mailing service
    /// </summary>
    /// <seealso cref="Mvc.Mailer.MailerBase" />
    public class UserMailer : MailerBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMailer"/> class.
        /// </summary>
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        /// <summary>
        /// Sends the mail for user to confirm registration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual MvcMailMessage ConfirmRegistration(User model)
        {
            ViewBag.BaseUrl = CurrentHttpContext.Request.Url.Authority;
            ViewBag.Hash = model.ConfirmHash;
            ViewBag.Id = model.Id;
            return Populate(x =>
            {
                x.Subject = "Confirm Registration";
                x.ViewName = "ConfirmRegistration";
                x.To.Add(model.EMail);
            });
        }

        /// <summary>
        /// Sends the mail for user to reset their password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual MvcMailMessage ResetPassword(User model)
        {
            ViewBag.EMail = model.EMail;
            ViewBag.Name = $"{model.FirstName} {model.LastName}";
            ViewBag.Hash = model.ConfirmHash.ToString();
            ViewBag.BaseUrl = CurrentHttpContext.Request.Url.Authority;
            return Populate(x =>
            {
                x.Subject = "Reset Your Password";
                x.ViewName = "ResetPassword";
                x.To.Add(model.EMail);
            });
        }

    }

}