﻿using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace instabot
{
    public class Routine
    {
        public static void First_Routine(User user)
        {
            var driver = Init_driver();
            HomePage homePage = new HomePage(driver, "https://www.instagram.com");
            LoginPage loginPage = homePage.GoToLoginPage();
            ProfilePage profilePage = loginPage.Login(user.id, user.password);
            PostPage post = profilePage.Find_Post("emalroni");
            if(user.like)
            {
                post.Click_Like();
            }
            if(user.save)
            {
                post.Click_Save();
            }
            if(user.target != null)
            {
                string msg = String.Format("@{0} {1}", user.target, user.message);
                post.Leave_Comment(user.target, msg);
            }
        }

        public static ChromeDriver Init_driver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            ChromeDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return driver;
        }
    }
}
