using System.Collections.Generic;
using System.Threading.Tasks;
using MeetUpAppAPI.Entities;
using MeetUpAppAPI.DTOs;

namespace MeetUpAppAPI.Interfaces
{
    public interface ILikesRepository
    {
        public Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        public Task<AppUser> GetUserWithLikes( int userId);
        public Task<IEnumerable<LikeDto>> GetUserLikes(string keyword, int userid);
    }
}