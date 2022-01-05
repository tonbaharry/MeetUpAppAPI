using System.Linq;
using AutoMapper;
using MeetUpAppAPI.DTOs;
using MeetUpAppAPI.Entities;

namespace MeetUpAppAPI.Helper
{
    public class AutoMapperProfile: Profile
    {
        //maps/matches entities of AppUser class to MemberDto
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(w=>w.PhotoUrl, x=>x.MapFrom(y=>y.Photos.FirstOrDefault(z=>z.IsMain).Url));
                //the above statement maps photourl on MemberDTO to the URL in the Photos Table (class)
            CreateMap<Photo, PhotoDto> ();
            //Mapper memberupdateDTO to AppUser model
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
        }
    }
}