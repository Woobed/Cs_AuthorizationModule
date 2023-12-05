using AuthorizationKP.Domain.Entity;
using AuthorizationKP.Domain.Response;
using AuthorizationKP.Domain.ViewModels.Users;
using AuthorizationKP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthorizationKP.Domain.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<IEnumerable<Users>>> GetUsers();

        Task<BaseResponse<Users>> GetById(int id);

        Task<BaseResponse<bool>> DeleteUser(int id);

        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);
        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
        Task<BaseResponse<bool>> TwoFactAuthenticate(TwoFactAuthenticationViewModel model1, string Login, string systemConfirmCode);

    }
}
