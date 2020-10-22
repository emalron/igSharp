using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace instabot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filename = "ids.xlsx";
            string dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, filename);
            var usersA = Manager.GetUsers(path, "A");
            var usersB = Manager.GetUsers(path, "B");
            var target = Manager.GetIndex(path, "C");
            for (int i=0; i<usersA.Count; i++)
            {
                usersA[i].target = usersB[i].id;
            }

            foreach(var user in usersA)
            {
                await Routine.First_RoutineAsync(user, target);
            }
            foreach(var user in usersB)
            {
                await Routine.Second_RoutineAsync(user);
            }
        }
    }
}
