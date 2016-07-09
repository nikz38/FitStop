using FitStop.Common.Exceptions;
using FitStop.Common.Helpers;
using FitStop.Data.UnitOfWork;
using FitStop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Core
{
    public class UserManager
    {
        /// <summary>
        /// Gets the user by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public User Get(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User user = uow.UserRepository.Find(u => u.Id == id, include: "UserSetting").FirstOrDefault();
                ValidationHelper.ValidateNotNull(user);

                return user;
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="query">The query to filter by.</param>
        /// <param name="getManagers">if set to <c>true</c> [is admin].</param>
        /// <returns></returns>
        public List<User> GetAll(string query, bool getManagers)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<User> users = uow.UserRepository.Search(query, getManagers);

                return users;
            }
        }

        /// <summary>
        /// Gets the user by specified email and password.
        /// </summary>
        /// <param name="eMail">The e mail.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">
        /// Wrong password!
        /// or
        /// Account not confirmed! Please, check your e-mail for confirmation.
        /// </exception>
        public User Get(string eMail, string password)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User user = uow.UserRepository.Find(u => u.EMail.ToLower().Trim() == eMail.ToLower().Trim(), include: "UserSetting").FirstOrDefault();
                ValidationHelper.ValidateNotNull(user);

                if (!PasswordHelper.ValidatePassword(password, user.Password))
                {
                    throw new ValidationException("Wrong password!");
                }

                if (!user.Active)
                {
                    throw new ValidationException("Account not confirmed! Please, check your e-mail for confirmation.");
                }

                return user;
            }
        }

        /// <summary>
        /// Gets the by e mail.
        /// </summary>
        /// <param name="eMail">The e mail.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">Account not confirmed! Please, check your e-mail for confirmation.</exception>
        public User GetByEMail(string eMail)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User user = uow.UserRepository.Find(u => u.EMail.ToLower().Trim() == eMail.ToLower().Trim(), include: "UserSetting").FirstOrDefault();
                ValidationHelper.ValidateNotNull(user);

                if (!user.Active)
                {
                    throw new ValidationException("Account not confirmed! Please, check your e-mail for confirmation.");
                }

                return user;
            }
        }

        /// <summary>
        /// Gets the user by hash.
        /// </summary>
        /// <param name="confirmHash">The confirm hash.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">Unable to confirm user! Token unparsable.</exception>
        public User GetByHash(string confirmHash)
        {
            Guid hash;

            if (!Guid.TryParse(confirmHash, out hash))
            {
                throw new ValidationException("Unable to confirm user! Token unparsable.");
            }

            using (UnitOfWork uow = new UnitOfWork())
            {
                User user = uow.UserRepository.Find(a => a.ConfirmHash == hash).FirstOrDefault();
                ValidationHelper.ValidateNotNull(user);

                return user;
            }
        }

        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="activate">if set to <c>true</c> [activate user].</param>
        /// <returns></returns>
        public User Register(User user, bool activate = false)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User existingUser = uow.UserRepository.Single(u => u.EMail.ToLower().Trim() == user.EMail.ToLower().Trim());
                ValidationHelper.ValidateEntityExist(existingUser);

                user.Active = activate;
                user.ConfirmHash = Guid.NewGuid();
                user.Password = PasswordHelper.CreateHash(user.Password);
                user.Role = Common.UserRole.AppUser;
                user.UserSetting = new UserSetting
                {
                    DailyCalorieIntake = 2000
                };

                uow.UserRepository.Insert(user);
                uow.Save();

                return user;
            }
        }

        /// <summary>
        /// Adds the user manager.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public User RegisterUserManager(User user)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User existingUser = uow.UserRepository.Single(u => u.EMail.ToLower().Trim() == user.EMail.ToLower().Trim());
                ValidationHelper.ValidateEntityExist(existingUser);

                user.Active = true;
                user.ConfirmHash = Guid.NewGuid();
                user.Password = PasswordHelper.CreateHash(user.Password);
                user.Role = Common.UserRole.UserManager;
                user.UserSetting = new UserSetting
                {
                    DailyCalorieIntake = 2000
                };

                uow.UserRepository.Insert(user);
                uow.Save();

                return user;
            }
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public User Update(User user)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User userFromDb = uow.UserRepository.GetById(user.Id);
                ValidationHelper.ValidateNotNull(userFromDb);

                userFromDb.FirstName = user.FirstName;
                userFromDb.LastName = user.LastName;

                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    if (PasswordHelper.ValidatePassword(user.Password, userFromDb.Password))
                    {
                        throw new ValidationException("This is the same password you had already!");
                    }
                    userFromDb.Password = PasswordHelper.CreateHash(user.Password);
                    userFromDb.ConfirmHash = null;
                }

                if (user.ConfirmHash.HasValue)
                {
                    userFromDb.ConfirmHash = user.ConfirmHash;
                }

                uow.UserRepository.Update(userFromDb);

                uow.Save();
                userFromDb.UserSetting = uow.UserSettingRepository.GetById(userFromDb.Id);

                return userFromDb;
            }
        }

        /// <summary>
        /// Toggles the active state of the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void ToggleActive(int userId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User userFromDb = uow.UserRepository.GetById(userId);
                ValidationHelper.ValidateNotNull(userFromDb);
                userFromDb.Active = !userFromDb.Active;

                uow.UserRepository.Update(userFromDb);
                userFromDb.UserSetting = uow.UserSettingRepository.GetById(userFromDb.Id);

                uow.Save();
            }
        }

        /// <summary>
        /// Updates the user settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void UpdateUserSettings(UserSetting settings)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                UserSetting userSettingFromDb = uow.UserSettingRepository.GetById(settings.UserId);
                ValidationHelper.ValidateNotNull(userSettingFromDb);
                userSettingFromDb.DailyCalorieIntake = settings.DailyCalorieIntake;

                uow.UserSettingRepository.Update(userSettingFromDb);

                uow.Save();
            }
        }

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Delete(User user)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User userFromDb = uow.UserRepository.GetById(user.Id);
                ValidationHelper.ValidateNotNull(userFromDb);
                uow.UserRepository.Delete(userFromDb);

                uow.Save();
            }
        }

        /// <summary>
        /// Confirms the registration.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="confirmHash">The confirm hash.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">
        /// Unable to confirm user! Token unparsable.
        /// or
        /// Unable to confirm user! Token invalid.
        /// </exception>
        public User ConfirmRegistration(int userId, string confirmHash)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                User existingUser = uow.UserRepository.GetById(userId);
                ValidationHelper.ValidateNotNull(existingUser);

                Guid hash;

                if (!Guid.TryParse(confirmHash, out hash))
                {
                    throw new ValidationException("Unable to confirm user! Token unparsable.");
                }

                if (existingUser.ConfirmHash != hash)
                {
                    throw new ValidationException("Unable to confirm user! Token invalid.");
                }

                existingUser.Active = true;

                uow.UserRepository.Update(existingUser);

                uow.Save();

                return existingUser;
            }
        }


    }
}
