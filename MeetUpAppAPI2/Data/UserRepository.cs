using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetUpAppAPI.DTOs;
using MeetUpAppAPI.Entities;
using MeetUpAppAPI.Helper;
using MeetUpAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetUpAppAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<MemberDto> GetMemberAsync(string username) //
        {
            return await _context.Users.Where(x=>x.UserName==username)
                            .Select(user=> new MemberDto
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                Age = user.GetAge(),
                                City = user.City,
                                Country = user.Country,
                                CreateDt = user.CreateDt,
                                Gender = user.Gender,
                                Interests = user.Interests,
                                Introduction = user.Introduction,
                                KnownAs = user.KnownAs,
                                LastActive = user.LastActive,
                                LookingFor = user.LookingFor,
                                //Photos = (ICollection<PhotoDto>)user.Photos,
                                PhotoUrl = user.Photos.ElementAt(0).Url
                            }).SingleOrDefaultAsync();
        }

        public async Task<PagedList<AppUser>> GetUserAsync(UserParams userParams)
        {
            //to add collections (photos) add an include statement
            var query =  _context.Users.Include(p=>p.Photos).AsNoTracking();
            return await PagedList<AppUser>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public  async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            //to add collections (photos) add an include statement
            return await _context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.UserName == username);
        }

        public Task<AppUser> GetUserByUsernameAsync(string username)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}