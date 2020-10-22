using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using TextCopy;

namespace instabot
{
    public class Page
    {
        protected ChromeDriver driver;
        public Page(ChromeDriver driver)
        {
            this.driver = driver;
        }
        public IWebElement Find_Element(ReadOnlyCollection<IWebElement> elements, string attribute, string text)
        {
            foreach(var element in elements)
            {
                bool isMatched = element.GetAttribute(attribute).Contains(text);
                if (isMatched)
                {
                    return element;
                }
            }
            return null;
        }
        public void Trial_Click(string selector, string attribute, string text)
        {
            int tries = 0;
            while(tries < 3)
            {
                var elements = this.driver.FindElements(By.CssSelector(selector));
                var element = this.Find_Element(elements, attribute, text);
                bool noElement = element == null;
                if (noElement)
                {
                    Thread.Sleep(3000);
                    tries++;
                } else
                {
                    element.Click();
                    return;
                }
            }
        }
        public void MoveToElement(IWebElement element)
        {
            var action = new Actions(this.driver).MoveToElement(element).Click();
        }
        public void Copy_Input(IWebElement target, string text)
        {
            ClipboardService.SetText(text);
            var action = new Actions(this.driver).MoveToElement(target).Click().KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control);
            action.Perform();
        }
    }
}
