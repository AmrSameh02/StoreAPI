using Store.Route.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Services.Contract
{
    public interface IUserService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> ChechEmailExistAsync(string email);
    } 
}
