using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace instabot
{
    public class Routine
    {
        public static async Task First_RoutineAsync(User user, string target)
        {
            var driver = Init_driver();
            HomePage homePage = new HomePage(driver, "https://www.instagram.com");
            LoginPage loginPage = homePage.GoToLoginPage();
            ProfilePage profilePage = loginPage.Login(user.id, user.password);
            PostPage post = profilePage.Find_Post(target);
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
                post.Leave_Comment(msg);
            }
            Thread.Sleep(5000);
            driver.Close();
            await Manager.Change_IP();
        }

        public static async Task Second_RoutineAsync(User user)
        {
            var driver = Init_driver();
            HomePage homePage = new HomePage(driver, "https://www.instagram.com");
            LoginPage loginPage = homePage.GoToLoginPage();
            ProfilePage profilePage = loginPage.Login(user.id, user.password);
            PostPage post = profilePage.Find_Calling(user);
            if (user.like)
            {
                post.Click_Like();
            }
            if (user.save)
            {
                post.Click_Save();
            }
            post.Answer(user);
            Thread.Sleep(5000);
            driver.Close();
            await Manager.Change_IP();
        }

        public static ChromeDriver Init_driver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            ChromeDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            return driver;
        }
    }
}
