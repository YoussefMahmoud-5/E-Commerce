using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransfereObeject.IdentityModule;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        // Login 
        // Take Email, Password
        // Return Token, Email, Display Name
        Task<UserDto> LoginAsync(LoginDto loginDto);
        // Register
        // Take Email, Password, Display Name, User Name , Phone Number
        // Return Token, Email, Diplay Name
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        // Check Email 
        // Take Email
        // Return bool
        Task<bool> CheckEmailAsync(string email);

        // Get Current User
        // Take Email 
        // Return UserDto (Token, Email, Diplay Name)
        Task<UserDto> GetCurrentUserAsync(string email);

        // Get Current User Address
        // Take Email 
        // Return AddressDto 
        Task<AddressDto> GetCurrentUserAddressAsync(string email);

        // Update Current User Address
        // Take Email, AddressDto
        // Return AddressDto
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email,  AddressDto addressDto);
    }
}
