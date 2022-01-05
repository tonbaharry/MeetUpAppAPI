using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MeetUpAppAPI.Interfaces;
using System;
using System.Security.Claims;

namespace MeetUpAppAPI.Helper
{
    public class LogUserActivity : IAsyncActionFilter  //t
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            else {
                //var userId = resultContext.HttpContext.User.GetUserId();
                var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
                var user = await repo.GetUserByIdAsync(userId);
                user.LastActive = DateTime.Now;
                await repo.SaveAllAsync();
            }
        }
    }
}