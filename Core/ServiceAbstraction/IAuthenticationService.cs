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
    }
}
