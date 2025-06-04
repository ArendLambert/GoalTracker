using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Core.Models;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Entities;
using GoalTrackerApp.Contracts;

namespace GoalTrackerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserModel>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<UserModel?>> GetUserById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                Console.WriteLine("NULL");
                return NotFound();
            }
            return Ok(user);
        }

        //[HttpPost]
        //[Route("add")]
        //public async Task<ActionResult> AddUser([FromBody] UserModel userModel)
        //{
        //    if (userModel == null || string.IsNullOrEmpty(userModel.Email) || string.IsNullOrEmpty(userModel.Password))
        //    {
        //        return BadRequest("Invalid user data.");
        //    }
        //    await _userService.AddAsync(userModel.Email, userModel.Password);
        //    return CreatedAtAction(nameof(GetUserById), new { id = userModel.Id }, userModel);
        //}

        [HttpPut]
        [Route("update")]
        [Authorize]
        public async Task<ActionResult> UpdateUser([FromBody] UserModel userModel)
        {
            UserModel? user = await _userService.GetByIdAsync(userModel.Id);
            if (userModel == null || userModel.Id == Guid.Empty || user == null)
            {
                return BadRequest("Invalid user data.");
            }
            user.Email = userModel.Email;
            user.IdThemeSet = userModel.IdThemeSet;
            await _userService.UpdateAsync(user);
            return NoContent();
        }

        [HttpPut]
        [Route("updatepassword/{password}/{idUser}")]
        [Authorize]
        public async Task<ActionResult> UpdateUserPassword(string password, Guid idUser)
        {
            UserModel? userModel = await _userService.GetByIdAsync(idUser);
            if (userModel == null || idUser == Guid.Empty)
            {
                return BadRequest("Invalid user data.");
            }
            userModel.Password = PasswordHasher.PasswordHasher.Generate(password);
            await _userService.UpdateAsync(userModel);
            return NoContent();
        }

        // TODO new update

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            if (!await _userService.ExistsAsync(id))
            {
                return NotFound();
            }
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("email/{email}")]
        [Authorize]
        public async Task<ActionResult<UserModel?>> GetUserByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("exists/{id:guid}")]
        [Authorize]
        public async Task<ActionResult<bool>> UserExists(Guid id)
        {
            bool exists = await _userService.ExistsAsync(id);
            return Ok(exists);
        }

        [HttpGet]
        [Route("existsbyemail/{email}")]
        public async Task<ActionResult<bool>> UserExistsByEmail(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginContract user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid login data.");
            }
            try
            {
                string token = await _userService.Login(user.Email, user.Password);
                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                };
                HttpContext.Response.Cookies.Append("goida", token, cookieOptions);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] UserLoginContract user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid registration data.");
            }
            try
            {
                await _userService.AddAsync(user.Email, user.Password);
                string token = await _userService.Login(user.Email, user.Password);
                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                };
                HttpContext.Response.Cookies.Append("goida", token, cookieOptions);
                return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}
