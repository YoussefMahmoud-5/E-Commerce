using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransfereObeject.IdentityModule;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Login
        [HttpPost("Login")] // POST : BaseUrl/Api/Authentication/Login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _serviceManager.authenticationService.LoginAsync(loginDto);
            return Ok(user);
        }
        // Register
        [HttpPost("Register")] // POST : BaseUrl/Api/Authentication/Register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _serviceManager.authenticationService.RegisterAsync(registerDto);
            return Ok(user);
        }
    }
}
