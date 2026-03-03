using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.API.Controllers.BackendForFrontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController
    {
        public TestAuthController()
        {
                
        }

        [HttpGet("anonymous")]
        public string GetSomethingForAnonymous()
        {
            return new("You're anonymous, and you did it!");
        }


        [Authorize(Roles = "Member")]
        [HttpGet("member")] 
        public string GetSomethingForMembers()
        {
            return new("You're a user, and you did it!");
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("driver")]
        public string GetSomethingForDrivers()
        {
            return new("You're a driver, and you did it!");
        }

        [Authorize(Roles = "Cook")]
        [HttpGet("cook")]
        public string GetSomethingForCooks()
        {
            return new("You're a cook, and you did it!");
        }   

        [Authorize(Roles = "Manager")]
        [HttpGet("manager")]
        public string GetSomethingForManagers()
        {
            return new("You're a manager, and you did it!");
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("owner")]
        public string GetSomethingForOwners()
        {
            return new("You're an owner, and you did it!");
        }


        [Authorize(Roles = "Manager, Member")]
        [HttpGet("managerormember")]
        public string GetSomethingForManagersOrMembers()
        {
            return new("You're a manager or member, and you did it!");
        }
    }
}
