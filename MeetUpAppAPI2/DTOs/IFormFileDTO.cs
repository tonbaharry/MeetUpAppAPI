using Microsoft.AspNetCore.Http;

namespace MeetUpAppAPI.DTOs
{
    public class IFormFileDTO
    {
        public string username { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}