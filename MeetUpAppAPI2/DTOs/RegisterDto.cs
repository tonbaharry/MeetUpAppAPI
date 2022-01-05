using System.ComponentModel.DataAnnotations;
using System;

namespace MeetUpAppAPI.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName {get;set;}
        [Required]
        [StringLength(20, MinimumLength =6)]
        public string Password {get;set;}
        [Required] 
        public string KnownAs { get;set;}
        [Required] 
        public string Gender { get;set;}
        [Required] 
        public DateTime DateOfBirth { get;set;}
        [Required] 
        public string City { get;set;}
        [Required] 
        public string Country { get;set;}
    }
}