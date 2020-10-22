using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using ClosedXML.Excel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Threading;
using System.Linq;

namespace instabot
{
    public class Manager
    {
        public static ChromeDriver Init()
        {
            return new ChromeDriver();
        }

        public static List<User> GetUsers(string path, string name)
        {
            using(var wb = new XLWorkbook(path))
            {
                var sheet = wb.Worksheet(name);
                var rows = sheet.RowsUsed();
                List<User> users = new List<User>();
                foreach(var row in rows)
                {
                    var user_ = CreateUser(row);
                    bool is_user = user_ != null;
                    if(is_user)
                    {
                        users.Add(user_);
                    }
                }
                return users;
            }
        }

        public static string GetIndex(string path, string name)
        {
            using (var wb = new XLWorkbook(path))
            {
                var sheet = wb.Worksheet(name);
                var row = sheet.RowsUsed();
                var cell = row.First().CellsUsed().First();
                var target = cell.GetString();
                bool is_null = String.IsNullOrWhiteSpace(target);
                if(is_null)
                {
                    return null;
                }
                return target;
            }
        }

        private static User CreateUser(IXLRow row)
        {
            User user_ = null;
            string id, password;
            bool invalid_user_info;
            Queue<string> queue = new Queue<string>();
            foreach(var cell in row.CellsUsed())
            {
                string data = cell.GetString();
                bool invalid_string = String.IsNullOrWhiteSpace(data);
                if (invalid_string)
                {
                    continue;
                }
                queue.Enqueue(cell.GetString());
            }
            id = queue.Dequeue();
            password = queue.Dequeue();
            invalid_user_info = String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(password);
            if (invalid_user_info)
            {
                return user_;
            }
            user_ = new User(id, password);
            foreach(var item in queue)
            {
                switch(item)
                {
                    case "좋아요":
                        user_.like = true;
                        break;
                    case "저장":
                        user_.save = true;
                        break;
                    default:
                        user_.message = item;
                        break;
                }
            }
            return user_;
        }

        public static string ADB_getPath()
        {
            string filename = "adb.exe";
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "adb");
            string path = Path.Combine(dir, filename);
            return path;
        }

        public static string ADB_Run()
        {
            string command = "devices";
            string path = ADB_getPath();
            var psInfo = Get_psInfo(path, command);

            Process p_ = Process.Start(psInfo);
            p_.WaitForExit();
            string output = p_.StandardOutput.ReadToEnd();

            return output;
        }

        private static ProcessStartInfo Get_psInfo(string path, string command)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = path;
            psInfo.Arguments = command;
            psInfo.UseShellExecute = false;
            psInfo.CreateNoWindow = true;
            psInfo.RedirectStandardOutput = true;

            return psInfo;
        }

        public static void ADB_Wifi(string command)
        {
            string filename = "adb.exe";
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "adb");
            string path = Path.Combine(dir, filename);
            var psInfo = Get_psInfo(path, command);
            bool is_connected = true;
            Process p_ = Process.Start(psInfo);
            p_.WaitForExit();
            Thread.Sleep(3000);
            while (is_connected)
            {
                is_connected = !Manager.Check_Internet_Connectivity();
            }
        }

        public static async Task<string> Get_MyIPAsync()
        {
            var httpClient = new HttpClient();
            var ip = await httpClient.GetStringAsync("https://api.ipify.org");
            return ip;
        }

        public static bool Check_Internet_Connectivity()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://google.com/generate_204"))
                    {
                        return true;
                    }
                }
            } catch
            {
                return false;
            }
        }

        public static async Task Change_IP()
        {
            bool is_changed = false;
            string prev_ip = await Get_MyIPAsync();
            string current_ip = null;
            while (!is_changed)
            {
                ADB_Wifi("shell svc wifi enable");
                ADB_Wifi("shell svc wifi disable");
                current_ip = await Get_MyIPAsync();
                is_changed = prev_ip != current_ip;
            }
            string result = String.Format("IP: {0}", current_ip);
            Console.WriteLine(result);
        }
    }
}
