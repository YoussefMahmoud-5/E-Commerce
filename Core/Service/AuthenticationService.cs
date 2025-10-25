using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransfereObeject.IdentityModule;

namespace Service
{
    internal class AuthenticationService(UserManager<ApplicationUser> _userManger,IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            // Check if Email is exist
            var user = await _userManger.FindByEmailAsync(loginDto.Email);
            if(user is null)
            {
                throw new UserNotFoundException(loginDto.Email);
            }
            // Check if Password Valid 
            var isPasswordValid = await _userManger.CheckPasswordAsync(user, loginDto.Password);
            if(isPasswordValid)
            {
                return new UserDto()
                {
                    Email = user.Email!,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            throw new UnAuthorizedException();            
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            // Mapping RegisterDto To Application User
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            // Create User
            var result = await _userManger.CreateAsync(user,registerDto.Password);
            if (result.Succeeded)
            {
                return new UserDto()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            else
            {
                throw new BadRequestException(result.Errors.Select(E => E.Description).ToList());
            }
        }
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Create Clames
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            var roles = await _userManger.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create Secrete Key 
            var secretKey = _configuration.GetSection("JwtOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds 
                );
             return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
