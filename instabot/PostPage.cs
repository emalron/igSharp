using DocumentFormat.OpenXml.Presentation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace instabot
{
    public class PostPage : Page
    {
        public PostPage(ChromeDriver driver) : base(driver)
        {

        }
        public PostPage Click_Like()
        {
            Trial_Click("section>span>button>div>span>svg", "aria-label", "좋아요");
            return new PostPage(this.driver);
        }
        public PostPage Click_Save()
        {
            Trial_Click("section>span>div>div>button>div>svg", "aria-label", "저장");
            return new PostPage(this.driver);
        }
        public PostPage Leave_Comment(string message)
        {
            var comment = this.driver.FindElement(By.CssSelector("section>div>form>textarea"));
            comment.Click();
            var comment2 = this.driver.FindElement(By.CssSelector("section>div>form>textarea"));
            Copy_Input(comment2, message);
            Thread.Sleep(1000);
            Trial_Click("section>div>form>button", "innerText", "게시");
            return new PostPage(this.driver);
        }
        public PostPage Answer(User user)
        {
            var comments = this.driver.FindElements(By.CssSelector("article>div>div>ul>ul"));
            foreach(var comment in comments)
            {
                var msg = comment.FindElement(By.CssSelector("div>li>div>div>div>span"));
                bool has_my_name = msg.GetAttribute("innerText").Contains(user.id);
                if(has_my_name)
                {
                    var button = comment.FindElement(By.CssSelector("div>li>div>div>div>div>div>button"));
                    var action = new Actions(this.driver).MoveToElement(button).Click();
                    action.Perform();
                    Leave_Comment(user.message);
                    return new PostPage(this.driver);
                }

            }
            return new PostPage(this.driver);
        }
    }
}
