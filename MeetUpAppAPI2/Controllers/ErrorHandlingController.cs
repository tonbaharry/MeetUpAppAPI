using System;
using MeetUpAppAPI.Data;
using MeetUpAppAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetUpAppAPI2.Controllers
{
    public class ErrorHandlingController: BaseAPIController
    {
        private readonly DataContext _context;
        public ErrorHandlingController(DataContext context){
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("Not Found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing ==null) return NotFound();

            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString(); //this will generate a null exception error
            return thingToReturn; 
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            
            return BadRequest("This was not a good request");
        }
    }
}