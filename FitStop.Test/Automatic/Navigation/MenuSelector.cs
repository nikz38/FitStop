using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Test.Automatic.Navigation
{
    public static class MenuSelector
    {
        public static void Select(string subMenu, string subMenuItem = null)
        {
            Driver.Instance.FindElement(By.LinkText(subMenu)).Click();
            if (subMenuItem != null)
            {
                Driver.Instance.FindElement(By.LinkText(subMenuItem)).Click();
            }
        }
    }

}
