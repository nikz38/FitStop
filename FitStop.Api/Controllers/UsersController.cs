using FitStop.Api.Helpers;
using FitStop.Api.Models;
using FitStop.Common;
using FitStop.Common.Exceptions;
using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;

namespace FitStop.Api.Controllers
{
    /// <summary>
    /// Users API controller
    /// </summary>
    /// <seealso cref="FitStop.Api.Controllers.BaseController" />
    public class UsersController : BaseController
    {
        /// <summary>
        /// Gets the user by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">You can only get your own profile!</exception>
        [TokenAuthorize]
        [HttpGet]
        public UserModel Get(int id)
        {
            if (CurrentUser.Role == UserRole.AppUser.ToString() && CurrentUser.Id != id)
            {
                throw new ValidationException("You can only get your own profile!");
            }

            User user = UserManager.Get(id);
            UserModel userModel = Mapper.Map(user);

            return userModel;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns></returns>
        [TokenAuthorize(Roles = "Administrator,UserManager")]
        [HttpGet]
        public List<UserModel> GetAll(string query = null)
        {
            List<User> users = UserManager.GetAll(query, getManagers: CurrentUser.Role == UserRole.Administrator.ToString());
            List<UserModel> usersModel = new List<UserModel>();
            foreach (var user in users)
            {
                usersModel.Add(Mapper.Map(user));
            }
            return usersModel;
        }

        /// <summary>
        /// Logins the user and produces session token.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns></returns>
        [HttpPost]
        public object Login(UserLoginModel model)
        {
            User user = UserManager.Get(model.EMail, model.Password);
            UserModel userModel = Mapper.Map(user);
            return new { User = userModel, Token = CreateLoginToken(user) };
        }

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns></returns>
        [HttpPost]
        public UserModel Register(UserRegisterModel model)
        {
            User user = Mapper.AutoMap<UserRegisterModel, User>(model);
            User registeredUser = UserManager.Register(user, activate: false);
            Mailer.ConfirmRegistration(registeredUser).Send();
            UserModel userModel = Mapper.Map(user);
            return userModel;
        }

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns></returns>
        [TokenAuthorize(Roles = "Administrator,UserManager")]
        [HttpPost]
        public UserModel RegisterUser(UserRegisterModel model)
        {
            User user = Mapper.AutoMap<UserRegisterModel, User>(model);
            User registeredUser = UserManager.Register(user, activate: true);
            UserModel userModel = Mapper.Map(user);
            return userModel;
        }

        /// <summary>
        /// Registers the specified user from the admin side.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns></returns>
        [TokenAuthorize(Roles = "Administrator")]
        [HttpPost]
        public UserModel RegisterUserManager(UserRegisterModel model)
        {
            User user = Mapper.AutoMap<UserRegisterModel, User>(model);
            User registeredUser = UserManager.RegisterUserManager(user);
            UserModel userModel = Mapper.Map(user);
            return userModel;
        }


        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">You can only edit your own profile!</exception>
        [TokenAuthorize]
        [HttpPut]
        public UserModel Update(User model)
        {
            User userFromDb = UserManager.Get(model.Id);

            if ((CurrentUser.Role == UserRole.AppUser.ToString() && CurrentUser.Id != model.Id)
                || (CurrentUser.Role == UserRole.UserManager.ToString() && CurrentUser.Id != model.Id && userFromDb.Role != UserRole.AppUser))
            {
                throw new ValidationException("You can only edit your own profile!");
            }

            User updatedUser = UserManager.Update(model);

            UserModel userModel = Mapper.Map(updatedUser);
            return userModel;
        }


        /// <summary>
        /// Toggles the active state for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        [TokenAuthorize(Roles = "Administrator,UserManager")]
        [HttpPut]
        public void ToggleActive(int userId)
        {
            User userFromDb = UserManager.Get(userId);

            if (CurrentUser.Role == UserRole.UserManager.ToString() && CurrentUser.Id != userId && userFromDb.Role != UserRole.AppUser)
            {
                throw new ValidationException("You cannot edit other managers profiles!");
            }

            UserManager.ToggleActive(userId);
        }

        /// <summary>
        /// Updates the user settings.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">You can only edit your own profile!</exception>
        [TokenAuthorize]
        [HttpPatch]
        public UserSetting UpdateUserSettings(UserSetting model)
        {
            User userFromDb = UserManager.Get(model.UserId);

            if ((CurrentUser.Role == UserRole.AppUser.ToString() && CurrentUser.Id != model.UserId)
                || (CurrentUser.Role == UserRole.UserManager.ToString() && CurrentUser.Id != model.UserId && userFromDb.Role != UserRole.AppUser))
            {
                throw new ValidationException("You can only edit your own profile!");
            }

            UserManager.UpdateUserSettings(model);

            return model;
        }

        /// <summary>
        /// Confirms the registration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [HttpGet]
        public RedirectResult ConfirmRegistration(int id, string hash)
        {
            User registeredUser = UserManager.ConfirmRegistration(id, hash);
            UserModel userModel = Mapper.AutoMap<User, UserModel>(registeredUser);
            return Redirect(WebConfigurationManager.AppSettings["webAppUrl"]);
        }

        /// <summary>
        /// Handles forgotten password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public bool ForgotPassword(UserResetPasswordModel model)
        {
            var user = UserManager.GetByEMail(model.EMail);
            user.ConfirmHash = Guid.NewGuid();
            UserManager.Update(user);

            Mailer.ResetPassword(user).Send();

            return true;
        }

        /// <summary>
        /// Reset password confirmation.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [HttpGet]
        public RedirectResult ResetPasswordConfirmation(string hash)
        {
            User user = UserManager.GetByHash(hash);
            return Redirect($"{WebConfigurationManager.AppSettings["webAppUrl"]}/reset-password/change-password/{hash}");
        }

        /// <summary>
        /// Sets the new password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public bool SetNewPassword(UserResetPasswordModel model)
        {
            User user = UserManager.GetByHash(model.ConfirmHash);
            user.Password = model.NewPassword;
            user.ConfirmHash = null;
            UserManager.Update(user);

            return true;
        }

        /// <summary>
        /// Deletes the user with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">
        /// Cannot delete your own profile!
        /// or
        /// You can only delete app users!
        /// </exception>
        [TokenAuthorize(Roles = "Administrator,UserManager")]
        [HttpDelete]
        public User Delete(int id)
        {
            User userFromDb = UserManager.Get(id);
            if (CurrentUser.Id == id)
            {
                throw new ValidationException("Cannot delete your own profile!");
            }

            if (CurrentUser.Role == UserRole.UserManager.ToString() && userFromDb.Role != UserRole.AppUser)
            {
                throw new ValidationException("You can only delete app users!");
            }

            UserManager.Delete(userFromDb);
            return userFromDb;
        }

        #region Private methods

        [NonAction]
        private string CreateLoginToken(User user)
        {
            UserJwtModel userModel = Mapper.MapJwt(user);
            userModel.ExpirationDate = DateTime.UtcNow.AddDays(1);

            string secretKey = WebConfigurationManager.AppSettings["JwtSecret"];
            string token = JWT.JsonWebToken.Encode(userModel, secretKey, JWT.JwtHashAlgorithm.HS256);
            return token;
        }

        #endregion

    }
}