using AuthorizationKP.Domain.Entity;
using AuthorizationKP.Domain.Enum;
using AuthorizationKP.Domain.Interfaces;
using AuthorizationKP.Domain.Response;
using AuthorizationKP.Domain.Service.Interfaces;
using AuthorizationKP.Domain.ViewModels.Users;
using AuthorizationKP.Helpers;
using AuthorizationKP.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthorizationKP.Domain.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IDbRepository _dbRepository;
        private readonly ITwoFactAuthentication _twoFact;

        public UserService(IDbRepository dbRepository, ITwoFactAuthentication twoFact)
        {
            _dbRepository = dbRepository;
            _twoFact = twoFact;
        }
        public async Task<BaseResponse<IEnumerable<Users>>> GetUsers()
        {
            var baseResponse = new BaseResponse<IEnumerable<Users>>();
            try
            {
                var users = await _dbRepository.GetAll();

                baseResponse.Data = users;
                baseResponse.StatusCode = StatusCode.Success;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Users>>()
                {
                    Description = $"[GetUsers] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }

        }

        public async Task<BaseResponse<Users>> GetById(int id)
        {
            try
            {
                var user = await _dbRepository.GetById(id);
                if (user == null)
                {
                    return new BaseResponse<Users>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound,
                    };
                }

                var data = new Users()
                {
                    Login = user.Login,
                    PasswordHash = user.PasswordHash,
                    NormalizedEmail = user.NormalizedEmail,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                };

                return new BaseResponse<Users>()
                {
                    Data = data,
                    StatusCode = StatusCode.Success,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Users>()
                {
                    Description = $"[GetById] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }




        public async Task<BaseResponse<bool>> DeleteUser(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var user = await _dbRepository.GetById(id);
                if (user == null)
                {
                    baseResponse.StatusCode = StatusCode.UserNotFound;
                    baseResponse.Description = "Пользователь не найден";
                    return baseResponse;
                }
                await _dbRepository.Delete(user);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[GetByLogin] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }


        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _dbRepository.GetByLogin(model.Login);
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                    };
                }

                user = new Users()
                {
                    
                    Login = model.Login,
                    PasswordHash = HashPasswordHelper.HashPassword(model.PasswordHash),
                    NormalizedEmail = model.NormalizedEmail.ToUpper(),
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Role = "User",
                    EmailConfirm = false,

            };

                //var confirmEmail = Convert.ToInt32(_twoFact.EmailConfirm(model.NormalizedEmail));
                
                await _dbRepository.Add(user);
                var result = Authenticate(user);
                model.systemConfirmCode = Convert.ToInt32(_twoFact.SendConfirmCode(user.NormalizedEmail));
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = Convert.ToString(model.systemConfirmCode),
                    StatusCode = StatusCode.Success,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Register] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
        public async Task<BaseResponse<bool>> TwoFactAuthenticate(TwoFactAuthenticationViewModel model1,string Login, string systemConfirmCode)
        {
            try
            {
                var user = await _dbRepository.GetByLogin(Login);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound,
                    };
                }
                
                if (model1.userConfirmCode == systemConfirmCode)
                {
                    user.EmailConfirm = true;
                    await _dbRepository.Update(user);

                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.Success,
                    };
                }
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }


        private ClaimsIdentity Authenticate(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,user.Role.ToString()),
            };

            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        }


        public async Task<BaseResponse<ClaimsIdentity>> Login (LoginViewModel model) 
        {
            try
            {
                var user = await _dbRepository.GetByLogin(model.Login);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден",
                    };
                }
                if (user.PasswordHash != HashPasswordHelper.HashPassword(model.PasswordHash))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль"
                    };
                }
                var result = Authenticate (user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.Success,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Login] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
        
        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = await _dbRepository.GetByLogin(model.Login);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                user.PasswordHash = HashPasswordHelper.HashPassword(model.NewPassword);
                await _dbRepository.Update(user);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.Success,
                    Description = "Пароль обновлен"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
    }
}
