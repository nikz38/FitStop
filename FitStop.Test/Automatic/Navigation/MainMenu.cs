using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Test.Automatic.Navigation
{
    public static class MainMenu
    {

        public static void Login()
        {
            MenuSelector.Select("Log In");
        }


        public static void Register()
        {
            MenuSelector.Select("Register");
        }

    }

}
