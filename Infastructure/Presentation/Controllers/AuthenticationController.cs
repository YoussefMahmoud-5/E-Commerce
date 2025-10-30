using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("Register")] // POST : BaseUrl/api/Authentication/Register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _serviceManager.authenticationService.RegisterAsync(registerDto);
            return Ok(user);
        }

        // Check Email
        [HttpGet("CheckEmail")] // GET : BaseUrl/api/Authentication/CheckEmail
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
           var result = await _serviceManager.authenticationService.CheckEmailAsync(email);
            return Ok(result);
        }
        // Get Current User
        [Authorize]
        [HttpGet("CurrentUser")] // GET : BaseUrl/api/Authentication/CurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var appUser = await _serviceManager.authenticationService.GetCurrentUserAsync(email!);
            return Ok(appUser);
        }

        // Get Current User Address
        [Authorize]
        [HttpGet("Address")] // GET : BaseUrl/api/Authentication/Address
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.authenticationService.GetCurrentUserAddressAsync(email!);
            return Ok(address);
        }

        // Update Current User Address
        [Authorize]
        [HttpPut("Address")] // PUT : BaseUrl/api/Authentication/Address
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var updateAddress = await _serviceManager.authenticationService.UpdateCurrentUserAddressAsync(email!, addressDto);
            return Ok(updateAddress);
        }
    }
}
