using System.Security.Claims;

namespace MeetUpAppAPI.Helper
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername (this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}