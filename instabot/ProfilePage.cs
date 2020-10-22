using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace instabot
{
    public class ProfilePage : Page
    {
        public ProfilePage(ChromeDriver driver) : base(driver)
        {

        }
        public PostPage Find_Post(string pattern)
        {
            var search = this.driver.FindElementByCssSelector("nav>div>div>div>div>input");
            search.SendKeys(pattern);
            Trial_Click("nav>div>div>div>div>div>div>div>a", "innerText", pattern);
            Trial_Click("article>div>div>div>div>a>div", "innerText", "");
            Thread.Sleep(3000);
            return new PostPage(this.driver);
        }

        public PostPage Find_Calling(User user)
        {
            Trial_Click("nav>div>div>div>div>div>div>a>svg", "aria-label", "활동 피드");
            Thread.Sleep(2000);
            var items = this.driver.FindElements(By.CssSelector("nav>div>div>div>div>div>div>div>div>div>div>div>div>div>div>div"));
            foreach(var item in items)
            {
                var msg = item.FindElement(By.CssSelector("span"));
                bool do_call_me = msg.GetAttribute("innerText").Contains(user.id);
                if(do_call_me)
                {
                    var links = item.FindElements(By.CssSelector("a"));
                    var link = links.Last();
                    link.Click();
                    Thread.Sleep(1000);
                    return new PostPage(this.driver);
                }
            }
            return new PostPage(this.driver);
        }
    }
}
