using MeetUpAppAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MeetUpAppAPI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MeetUpAppAPI.Interfaces;
using MeetUpAppAPI.DTOs;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MeetUpAppAPI.Helper;

namespace MeetUpAppAPI2.Controllers
{
    [Authorize]
    public class UsersController: BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            
            var users = await _userRepository.GetUserAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, 
                                        users.TotalPages);
            //map result of type AppUsers to MemberDto using Mapper
           // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")] //
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            
            var user = await _userRepository.GetUserByUserNameAsync(username);

            return _mapper.Map<MemberDto>(user);
        }

        [HttpPut] 
        public async Task<ActionResult> UpdateUser(MemberUpdateDto model)
        {
            var user = await _userRepository.GetUserByUserNameAsync(model.UserName);
            _mapper.Map(model, user);
            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) 
            {
                return NoContent();
            }
            else {
                return BadRequest("UPdate failed");
            }

        }

        [AllowAnonymous]
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUserNameAsync(username);
            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            //checks to see if it's the user's first photo. If it is photo is set as main photo
            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;

            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
                //return _mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("GetUser", new{username = user.UserName},  _mapper.Map<PhotoDto>(photo));
            else
                return BadRequest("Unable to add photo");
        }

        [HttpPut("set-profile-photo/{pictureId}")]
        public async Task<ActionResult> SetProfilePhoto(int pictureId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var photo = user.Photos.FirstOrDefault(x=>x.Id == pictureId);

            if(photo.IsMain) return BadRequest("Picture already set as profile picture");

            var currentProfilePic = user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currentProfilePic != null ) currentProfilePic.IsMain = false;
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Profile picture change failed");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var photo = user.Photos.FirstOrDefault(x=> x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null )
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}