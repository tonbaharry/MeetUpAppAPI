   using System.Threading.Tasks;
using MeetUpAppAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using System.Collections.Generic;
using MeetUpAppAPI.DTOs;
using MeetUpAppAPI.Entities;

namespace MeetUpAppAPI2.Controllers
{
    [Authorize]
    public class LikesController: BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            //int sourceUserId =  await _userRepository.GetUserIdByName(username);
            int sourceUserId  =  int.Parse( User.FindFirst(ClaimTypes.NameIdentifier)?.Value );
            var likedUser = await _userRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
             
            if(likedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("Self like disabled");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            
            if(userLike != null) return BadRequest("You have already liked this profile");

            userLike = new UserLike{
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if(await _userRepository.SaveAllAsync()) {
                return Ok();
            }

            return BadRequest("Attempt to Like User Failed");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string keyword)
        {
            int UserId =  int.Parse( User.FindFirst(ClaimTypes.NameIdentifier)?.Value );
            var users = await  _likesRepository.GetUserLikes(keyword, UserId);
            return Ok(users);
        }
    }
}