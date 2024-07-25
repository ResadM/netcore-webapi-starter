using DataAccess.Services;
using DataModel.Entities;
using DataModel.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;


namespace Business.Manager
{
    public class AccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;
        public AccountManager(UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<(bool status, string errorMessage, IEnumerable<IdentityError>? identityErrors, ApplicationUser? user)> RegisterUser(RegisterDto registerDto)
        {
            try
            {

                var user = new ApplicationUser() { UserName = registerDto.UserName, Email = registerDto.Email, Name = registerDto.Name, Lastname = registerDto.Lastname };

                var userResult = await _userManager.CreateAsync(user, registerDto.Password!);

                if (!userResult.Succeeded)
                    return (false, "", userResult.Errors, user);

                string userRole = "Member";

                var roleResult = await _userManager.AddToRoleAsync(user, userRole);
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return (false, "", roleResult.Errors, user);
                }
                return (true, "", null, user);

            }
            catch (Exception ex)
            {
                return (false, "Server Error", null, null);
            }
        }

        public async Task<BusinessLayerResult<UserDto>> LoginUser(LoginDto loginDto)
        {
            var userDto = new BusinessLayerResult<UserDto>();
            try
            {
                userDto.Data = new UserDto();
                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    userDto.AddError("Username or password is incorrect.");
                }
                else
                {
                    var userRole = await _userManager.GetRolesAsync(user);
                    string currentRole = "";
                    if (userRole != null && userRole.Count() > 0)
                    {
                        currentRole = userRole[0].ToString();
                    }
                    else
                    {
                        userDto.AddError("User roles not found.");
                    }
                    Guid.TryParse(user.Id, out Guid userGuid);
                    userDto.Data.ID = userGuid;
                    userDto.Data.Name = user.Name;
                    userDto.Data.Lastname = user.Lastname;
                    userDto.Data.UserName = user.UserName;
                    userDto.Data.Email = user.Email;
                    userDto.Data.UserRole = currentRole;
                    userDto.Data.Token = await _tokenService.GenerateToken(user);
                }
            }
            catch (Exception ex)
            {
                userDto.AddError("User login failed");
            }
            return userDto;
        }


    }
}
