using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace instabot
{
    public class LoginPage : Page
    {
        public LoginPage(ChromeDriver driver) : base(driver)
        {

        }
        public ProfilePage Login(string username, string password)
        {
            var inputs = this.driver.FindElementsByCssSelector("div>div>label>input");
            Copy_Input(inputs[0], username);
            Copy_Input(inputs[1], password);
            var buttons = this.driver.FindElements(By.CssSelector("form>div>div>button"));
            bool noButtons = buttons == null || buttons.Count == 0;
            if(noButtons)
            {
                throw new ArgumentNullException("no buttons");
            }
            var button = Find_Element(buttons, "innerText", "로그인");
            bool isButton = button != null;
            if (isButton)
            {
                var action = new Actions(this.driver).MoveToElement(button).Click();
                action.Perform();
                Trial_Click("div>div>button", "innerText", "나중에 하기");
                Trial_Click("div>div>button", "innerText", "나중에 하기");
                return new ProfilePage(this.driver);
            }
            else
            {
                throw new ArgumentNullException("no button");
            }
        }
    }
}
