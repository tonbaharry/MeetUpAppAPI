using MeetUpAppAPI.Entities;

namespace MeetUpAppAPI.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}