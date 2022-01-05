using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MeetUpAppAPI.Data;
using MeetUpAppAPI.DTOs;
using MeetUpAppAPI.Entities;
using MeetUpAppAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetUpAppAPI2.Controllers
{
    public class AccountController:BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName))
            {
                return BadRequest ("User Name is not available");
            }
            else {
                
                using var hash = new HMACSHA512();
                var user = new AppUser
                {
                    UserName = registerDto.UserName,
                    PasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hash.Key,
                    City = registerDto.City,
                    Country = registerDto.Country,
                    CreateDt = System.DateTime.Now,
                    DateOfBirth = registerDto.DateOfBirth,
                    Gender = registerDto.Gender,
                    KnownAs = registerDto.KnownAs
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return new UserDto {
                    userName = user.UserName, 
                    Token = _tokenService.CreateToken(user),
                    //PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                    KnownAs = user.KnownAs
                };
                
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDTO dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName==dto.UserName);
            if(user==null) return Unauthorized("User Not Found");
            //pass passwordsalt as key to regenerate the passwordhash
            using var hash = new HMACSHA512(user.PasswordSalt);
            var regeneratedPasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            for(int i = 0; i < regeneratedPasswordHash.Length; i++)
            {
                if(regeneratedPasswordHash[i] != user.PasswordHash[i]) return Unauthorized("Incorrect Password");
            }
            var response = new UserDto {
                            userName = user.UserName, 
                            Token = _tokenService.CreateToken(user),
                            KnownAs = user.KnownAs
                        };
            return response;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync( x => x.UserName == username.ToLower());
        }
    }
}
