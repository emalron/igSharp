using instabot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace ibTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Page_FindElement()
        {
            // Arrange
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Page page = new Page(driver);

            // Action
            driver.Url = "https://www.naver.com";
            var elements = driver.FindElements(By.CssSelector("input"));
            var element = page.Find_Element(elements, "name", "query");
            var expected = "검색어 입력";
            var actual = element.GetAttribute("title");

            // Assert
            Assert.AreEqual(expected, actual);
            driver.Close();
        }

        [TestMethod]
        public void Test_Page_TrialClick()
        {
            // Arrange
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Page page = new Page(driver);
            var selector_button = "#search_btn";
            var selector_result = "#notfound > div > p";

            // Action
            driver.Url = "https://www.naver.com";
            page.Trial_Click(selector_button, "type", "submit");
            var result = driver.FindElement(By.CssSelector(selector_result));
            var expected = "\'\'에 대한 검색결과가 없습니다.";
            var actual = result.GetAttribute("innerText");

            // Assert
            Assert.AreEqual(expected, actual);
            driver.Close();
        }

        [TestMethod]
        public void Test_Page_MoveToElement()
        {
            // Arrange
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Page page = new Page(driver);
            string selector = "#query";

            // Action
            driver.Url = "https://www.naver.com";
            var element = driver.FindElement(By.CssSelector(selector));
            page.MoveToElement(element);
        }

        [TestMethod]
        public void Test_Page_CopyInputToElement()
        {
            // Arrange
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Page page = new Page(driver);
            string selector = "#query";
            string query = "라센";

            // Action
            driver.Url = "https://www.naver.com";
            var element = driver.FindElement(By.CssSelector(selector));
            page.Copy_Input(element, query);
        }

        [TestMethod]
        public void Test_Manager_GetUsers_OpenExcelFile()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string name = "A";
            List<User> users = null;

            // Action
            try
            {
                users = Manager.GetUsers(path, name);
            } catch(Exception e)
            {
                string message = String.Format("{0}\n{1}", path, e.Message);
                Assert.Fail(message);
            }

            // Assert
            bool invalid_users = users == null;
            if(invalid_users)
            {
                Assert.Fail("Unexpected result: List users is null");
            }
        }

        [TestMethod]
        public void Test_Manager_GetUsers_UserData()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string name = "A";
            string expected_message = "불금인데 갈래?";
            string expected_id = "testo";
            string expected_password = "t1@";
            List<User> users = null;

            // Action
            try
            {
                users = Manager.GetUsers(path, name);
            }
            catch (Exception e)
            {
                string message = String.Format("{0}\n{1}", path, e.Message);
                Assert.Fail(message);
            }
            string actual_message = users[1].message;
            string actual_id = users[1].id;
            string actual_password = users[1].password;
            bool actual_like = users[1].like;
            bool acutal_save = !users[1].save;

            // Assert
            bool is_valid_user = expected_id == actual_id && expected_password == actual_password && expected_message == actual_message && acutal_save && actual_like;
            Assert.IsTrue(is_valid_user);
        }

        [TestMethod]
        public void Test_LoginPage_Login()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string name = "A";
            var user = Manager.GetUsers(path, name)[0];

            // Action
            try
            {
                var driver = Routine.Init_driver();
                HomePage hpage = new HomePage(driver, "https://www.instagram.com");
                LoginPage loginPage = hpage.GoToLoginPage();
                loginPage.Login(user.id, user.password);
            } catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
