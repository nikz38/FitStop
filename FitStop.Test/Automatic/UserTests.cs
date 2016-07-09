using FitStop.Test.Automatic.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Test.Automatic
{
    [TestClass]
    public class UserTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Driver.Initialize("http://localhost:6002/");
            LoginPage.GoTo();
            LoginPage.LoginAsAdmin();
        }

        [TestMethod]
        public void LoginAsAdmin()
        {
            Assert.IsTrue(UsersPage.IsAt, "Failed to login.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Driver.Close();
        }
    }

}
