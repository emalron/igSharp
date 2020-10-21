using System;
using System.Collections.Generic;
using System.Text;

namespace instabot
{
    public class User
    {
        public string id;
        public string password;
        public bool like;
        public bool save;
        public string target;
        public string message;

        public User(string id, string password)
        {
            this.id = id;
            this.password = password;
            this.like = false;
            this.save = false;
        }
    }
}
