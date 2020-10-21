using System;
using System.IO;
using System.Threading;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace instabot
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            string name = "A";
            var usersA = Manager.GetUsers(path, name);

            usersA.ForEach((user) => { Routine.First_Routine(user); });
        }
    }
}
