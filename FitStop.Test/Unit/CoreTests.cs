using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitStop.Core;
using FitStop.Entities;
using FitStop.Common.Exceptions;

namespace FitStop.Test.Unit
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void UserIntegrationTest()
        {
            UserManager manager = new UserManager();
            var user = new User
            {
                EMail = "testemail@test.com",
                FirstName = "Test",
                LastName = "Test",
                Password = "AwesomePass123"
            };

            var registeredUser = manager.Register(user);

            Assert.IsTrue(registeredUser.Id > 0, "Failed to create user.");
            Assert.IsNotNull(registeredUser.UserSetting, "Failed to edit user settings.");
            Assert.IsTrue(registeredUser.UserSetting.DailyCalorieIntake == 2000, "Failed to assign correct default user settings.");

            registeredUser.FirstName = "EditedTest";
            var updatedUser = manager.Update(registeredUser);

            Assert.IsTrue(updatedUser.FirstName == "EditedTest", "Failed to update user.");

            var userFromDb = manager.Get(updatedUser.Id);

            Assert.IsNotNull(userFromDb, "Failed to get the user.");
            Assert.IsTrue(userFromDb.FirstName == "EditedTest", "User changes were not saved.");

            manager.Delete(userFromDb);
            try
            {
                var deletedUser = manager.Get(userFromDb.Id);
                Assert.IsNull(userFromDb, "User was not deleted.");
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Message == "User does not exist!", "User was not deleted.");
            }
        }
    }
}
