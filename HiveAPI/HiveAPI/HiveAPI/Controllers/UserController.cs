using HiveAPI.Data;
using HiveAPI.Models;
using HiveAPI.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HiveAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly HiveApiContext _context;
        private readonly AppSettings _settings;

        public UserController(HiveApiContext context, IOptions<AppSettings> appOptions)
        {
            _context = context;
            _settings = appOptions.Value;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _context.Users.Select(u => new UserDto(u)).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id.Equals(id));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] User loginUser)
        {
            var user = await _context.Users.Where(u => (u.Username.Equals(loginUser.Username) || u.Email.Equals(loginUser.Email)) && PasswordHasher.CheckMatch(u.Password,loginUser.Password)).SingleOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Zły login lub hasło");
            }

            var token = JWTTokenCreator.GetNewToken(user.Id.ToString(), _settings.SecurityKey, 60);

            return Ok(new
            {
                user = new UserDto(user),
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost(Name = "CreateUser")]
        [AllowAnonymous]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] User createUser)
        {
            var user = await _context.Users.Where(u => (u.Username.Equals(createUser.Username) || u.Email.Equals(createUser.Email))).SingleOrDefaultAsync();
            if (user != null)
            {
                return BadRequest("Użytkownik już istnieje");
            }

            if (!IsEmailValid(createUser.Email) || IsPasswordValid(createUser.Password))
            {
                return BadRequest("Email jest nieprawidłowy, lub hasło krótsze niż 6 znaków");
            }


            var newUser = new User(createUser.Username, createUser.Email, PasswordHasher.CalculateHash(createUser.Password));
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = JWTTokenCreator.GetNewToken(newUser.Username, _settings.SecurityKey, 60);

            return Ok(new
            {
                user = new UserDto(newUser),
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto user, [FromBody] string password)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }

            var dbUser = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
            if (dbUser == null)
            {
                return NotFound();
            }

            dbUser.Username = user.Username;
            dbUser.Email = user.Email;
            dbUser.Password = password;

            _context.Users.Update(dbUser);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
            if (dbUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        bool IsEmailValid(string email)
        {
            var validator = new EmailAddressAttribute();
            return validator.IsValid(email);
        }

        bool IsPasswordValid(string password)
        {
            return password.Length >= 6;
        }

    }
}
