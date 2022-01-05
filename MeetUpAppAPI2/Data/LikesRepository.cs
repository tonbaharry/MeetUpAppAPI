using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetUpAppAPI.DTOs;
using MeetUpAppAPI.Entities;
using MeetUpAppAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetUpAppAPI.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string keyword, int userid)
        {
            var users = _context.Users.OrderBy(x=>x.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(keyword == "liked")
            {
                likes = likes.Where(x=>x.SourceUserId == userid);
                users = likes.Select(x=>x.LikedUser);
            }

            if(keyword == "likedBy")
            {
                likes = likes.Where(x=>x.LikedUserId == userid);
                users = likes.Select(x=>x.SourceUser);
            }

            return await users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user. GetAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p=>p.IsMain).Url,
                City = user.City,
                Id = user.Id
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(x=>x.LikedUsers)
            .FirstOrDefaultAsync(x=>x.Id == userId);
        }
    }
}