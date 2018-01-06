using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI.Models
{
    public class User
    {
        public User()
        {

        }

        public User(string username, string email, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            Password = password;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
