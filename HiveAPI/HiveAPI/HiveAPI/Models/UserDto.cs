using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI.Models
{
    public class UserDto
    {
        public UserDto(Guid id,string name, string email)
        {
            Id = id;
            Username = name;
            Email = email;
        }

        public UserDto(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

    }
}
