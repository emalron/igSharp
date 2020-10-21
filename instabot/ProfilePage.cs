using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
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
    }
}
