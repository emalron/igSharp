using DocumentFormat.OpenXml.Presentation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
            Thread.Sleep(1000);
            return new PostPage(this.driver);
        }
        public PostPage Click_Save()
        {
            Trial_Click("section>span>div>div>button>div>svg", "aria-label", "저장");
            Thread.Sleep(1000);
            return new PostPage(this.driver);
        }
        public PostPage Leave_Comment(string target, string message)
        {
            var comment = this.driver.FindElement(By.CssSelector("section>div>form>textarea"));
            comment.Click();
            Thread.Sleep(1000);
            var comment2 = this.driver.FindElement(By.CssSelector("section>div>form>textarea"));
            Copy_Input(comment2, message);
            Thread.Sleep(1000);
            Trial_Click("section>div>form>button", "innerText", "게시");
            Thread.Sleep(1000);
            return new PostPage(this.driver);
        }
    }
}
