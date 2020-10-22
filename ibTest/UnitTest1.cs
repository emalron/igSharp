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
using System.Reflection.PortableExecutable;

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

        [TestMethod]
        public void Test_Routine_FirstRoutine()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string name = "A";
            var usersA = Manager.GetUsers(path, name);
            var usersB = Manager.GetUsers(path, "B");
            var target = Manager.GetIndex(path, "C");
            for (int i = 0; i < usersA.Count; i++)
            {
                usersA[i].target = usersB[i].id;
            }

            // Action
            usersA.ForEach(async (user) => { await Routine.First_RoutineAsync(user, target); });
        }

        [TestMethod]
        public void Test_Manager_GetIndex()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string expected = "emalroni";

            // Action
            string actual = Manager.GetIndex(path, "C");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_PostPage_LeaveComment()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string name = "A";
            var usersA = Manager.GetUsers(path, name);
            var usersB = Manager.GetUsers(path, "B");
            for (int i = 0; i < usersA.Count; i++)
            {
                usersA[i].target = usersB[i].id;
            }
            var user = usersA[0];
            // Action
            var driver = Routine.Init_driver();
            HomePage homePage = new HomePage(driver, "https://www.instagram.com");
            LoginPage loginPage = homePage.GoToLoginPage();
            ProfilePage profilePage = loginPage.Login(user.id, user.password);
            PostPage post = profilePage.Find_Post("emalroni");
            if (user.like)
            {
                post.Click_Like();
            }
            if (user.save)
            {
                post.Click_Save();
            }
            if (user.target != null)
            {
                string msg = String.Format("@{0} {1}", user.target, user.message);
                post.Leave_Comment(msg);
            }
            Thread.Sleep(5000);
            driver.Close();
        }


        [TestMethod]
        public void Test_Routine_SecondRoutine()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            var usersA = Manager.GetUsers(path, "A");
            var usersB = Manager.GetUsers(path, "B");
            for (int i = 0; i < usersA.Count; i++)
            {
                usersA[i].target = usersB[i].id;
            }

            // Action
            usersB.ForEach(async (user) => { await Routine.Second_RoutineAsync(user); });
        }

        [TestMethod]
        public void Test_ProfilePage_FindCalling()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            var user = Manager.GetUsers(path, "B")[0];

            // Action
            var driver = Routine.Init_driver();
            HomePage homePage = new HomePage(driver, "https://www.instagram.com");
            LoginPage loginPage = homePage.GoToLoginPage();
            ProfilePage profilePage = loginPage.Login(user.id, user.password);
            PostPage post = profilePage.Find_Calling(user);
        }

        [TestMethod]
        public void Test_PostPage_Answer()
        {
            // Arrange
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            var user = Manager.GetUsers(path, "B")[0];
            var driver = Routine.Init_driver();
            HomePage homePage = new HomePage(driver, "https://www.instagram.com");
            LoginPage loginPage = homePage.GoToLoginPage();
            ProfilePage profilePage = loginPage.Login(user.id, user.password);
            PostPage post = profilePage.Find_Calling(user);

            // Action
            post.Answer(user);
        }

        [TestMethod]
        public void Test_Manager_ADB_Run()
        {
            // Action
            string output = Manager.ADB_Run();

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void Test_Manager_Internet_Connectivity()
        {
            // Action
            var actual = Manager.Check_Internet_Connectivity();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public async Task Test_Manager_ADB_Wifi()
        {
            // Arrange
            string expected = "164.125.48.38";
            bool is_connected = true;

            // Action
            Manager.ADB_Wifi("shell svc wifi enable");
            while (is_connected)
            {
                is_connected = !Manager.Check_Internet_Connectivity();
            }
            string actual = await Manager.Get_MyIPAsync();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Test_Manager_IP_CHANGE()
        {
            await Manager.Change_IP();
        }
    }
}
