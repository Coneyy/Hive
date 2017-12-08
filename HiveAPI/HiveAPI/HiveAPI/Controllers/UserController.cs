using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiveAPI.Models;
using HiveAPI.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HiveAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly HiveApiContext _context;

        public UserController(HiveApiContext context)
        {         
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] User loginUser)
        {
            var user = _context.Users.Where(u => (u.Username.Equals(loginUser.Username) || u.Email.Equals(loginUser.Email)) && u.Password.Equals(loginUser.Password)).SingleOrDefault();
            if (user == null)
            {
                return NoContent();
            }

            return new ObjectResult(new UserDto(user));
        }

        [HttpPost(Name = "CreateUser")]
        [Route("create")]
        public IActionResult Create([FromBody] User createUser)
        {
            var user = _context.Users.Where(u => (u.Username.Equals(createUser.Username) || u.Email.Equals(createUser.Email)) && u.Password.Equals(createUser.Password)).SingleOrDefault();
            if (user != null)
            {
                return BadRequest();
            }

            var newUser = new User(createUser.Username, createUser.Email, createUser.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return CreatedAtRoute("CreateUser", new UserDto(newUser));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UserDto user, [FromBody] string password)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }

            var dbUser = _context.Users.FirstOrDefault(t => t.Id == id);
            if (dbUser == null)
            {
                return NotFound();
            }
            
            dbUser.Username = user.Username;
            dbUser.Email = user.Email;
            dbUser.Password = password;

            _context.Users.Update(dbUser);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var dbUser = _context.Users.FirstOrDefault(t => t.Id == id);
            if (dbUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(dbUser);
            _context.SaveChanges();
            return new NoContentResult();
        }

    }
}
