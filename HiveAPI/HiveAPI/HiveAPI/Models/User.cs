using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required, MaxLength(80)]
        public string Username { get; set; }
        [Required, MaxLength(80)]
        public string Email { get; set; }
        [Required, MaxLength(180)]
        public string Password { get; set; }
    }
}
