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
using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransfereObeject.IdentityModule;

namespace Service
{
    internal class AuthenticationService(UserManager<ApplicationUser> _userManger,IConfiguration _configuration,IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
             var user = await _userManger.FindByEmailAsync(email);
            return user is not null;
        }
        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user is null)
                throw new UserNotFoundException(email);
            return new UserDto()
            {
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
        }
        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManger.Users.Include(U => U.Address)
                                              .FirstOrDefaultAsync(U => U.Email == email);
            if (user is null)
                throw new UserNotFoundException(email);
            if(user.Address is null)
                throw new AddressNotFoundException(user.UserName!);
            return _mapper.Map<Address,AddressDto>(user.Address);
        }
        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManger.Users.Include(U => U.Address)
                                              .FirstOrDefaultAsync(U => U.Email == email);
            if (user is null)
                throw new UserNotFoundException(email);
            if(user.Address is not null) // Update Address
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
            }
            else // Add New Address
            {
                user.Address = _mapper.Map<AddressDto,Address>(addressDto);
            }
            await _userManger.UpdateAsync(user);
            return _mapper.Map<Address,AddressDto>(user.Address);
        }

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
