using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Route.APIs.Errors;
using Store.Route.APIs.Extentions;
using Store.Route.Core.DTOs.Auth;
using Store.Route.Core.Entities.Identity;
using Store.Route.Core.Services.Contract;
using Store.Route.Service.Services.Tokens;
using System.Security.Claims;

namespace Store.Route.APIs.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(
            IUserService userService,
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")] //POST /api/Accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
        
            return Ok(user);
        }

        [HttpPost("register")] //POST /api/Accounts/login
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Registeration"));

            return Ok(user);
        }


        [Authorize]
        [HttpGet("GetCurrentUser")] //GET /api/Accounts/GetCurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });

        }

        [Authorize]
        [HttpGet("address")] //GET /api/Accounts/address
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        { 
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));

        }



    }
}
