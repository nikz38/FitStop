using FitStop.Api.Mailers;
using FitStop.Api.Models;
using FitStop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FitStop.Api.Controllers
{
    /// <summary>
    /// Base API controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class BaseController : ApiController
    {
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public UserJwtModel CurrentUser { get; set; }

        private UserManager userManager;
        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        protected UserManager UserManager { get { return userManager ?? (userManager = new UserManager()); } }

        private MealManager mealManager;
        /// <summary>
        /// Gets the meal manager.
        /// </summary>
        /// <value>
        /// The meal manager.
        /// </value>
        protected MealManager MealManager { get { return mealManager ?? (mealManager = new MealManager()); } }

        private UserMailer mailer;
        /// <summary>
        /// Gets the mailer.
        /// </summary>
        /// <value>
        /// The mailer.
        /// </value>
        protected UserMailer Mailer { get { return mailer ?? (mailer = new UserMailer()); } }
    }
}
