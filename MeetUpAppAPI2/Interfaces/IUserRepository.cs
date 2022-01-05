using System.Collections.Generic;
using System.Threading.Tasks;
using MeetUpAppAPI.DTOs;
using MeetUpAppAPI.Entities;
using MeetUpAppAPI.Helper;

namespace MeetUpAppAPI.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<bool> SaveAllAsync();
         Task<PagedList<AppUser>> GetUserAsync(UserParams userParams);
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUserNameAsync(string username);

         Task<AppUser> GetUserByUsernameAsync(string username);
         Task<MemberDto> GetMemberAsync(string username);
    }
}