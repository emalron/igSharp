using OpenQA.Selenium.Chrome;
using System.Threading;

namespace instabot
{
    public class HomePage : Page
    {
        public HomePage(ChromeDriver driver, string url) : base(driver)
        {
            this.driver.Url = url;
        }

        public LoginPage GoToLoginPage()
        {
            Thread.Sleep(5);
            return new LoginPage(this.driver);
        }
    }
}
