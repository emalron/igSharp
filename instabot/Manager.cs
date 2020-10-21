using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using ClosedXML.Excel;

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
    }
}
