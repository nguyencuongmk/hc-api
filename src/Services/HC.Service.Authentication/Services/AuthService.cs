using HC.Foundation.Common.Attributes;
using HC.Foundation.Common.Helpers;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Entities;
using HC.Service.Authentication.Helpers;
using HC.Service.Authentication.Models.Requests;
using HC.Service.Authentication.Models.Responses;
using HC.Service.Authentication.Services.IServices;
using HC.Service.Authentication.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static HC.Foundation.Common.Constants.Constants;

namespace HC.Service.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Assign role to User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<string> AssignRole(string username, string roleName)
        {
            #region Verify Request

            var message = string.Empty;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(roleName))
            {
                message = Message.NOT_ENOUGH_INFO;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _unitOfWork.UserRepository.FindSingle
            (
                expression: x => x.UserName == username,
                includes: q => q.Include(x => x.Roles)
            );

            if (user != null)
            {
                if (!await _unitOfWork.RoleRepository.IsExists(x => x.Name == roleName))
                {
                    message = Message.NOT_EXISTS_ROLE;
                    return message;
                }

                var role = await _unitOfWork.RoleRepository.FindSingle(x => x.Name == roleName);

                if (role != null)
                {
                    if (user.Roles.Contains(role))
                    {
                        message = Message.GRANTED_ROLE;
                        return message;
                    }

                    var isRoleAdded = await _unitOfWork.UserRepository.AddToRoleAsync(user, role);

                    if (!isRoleAdded)
                    {
                        message = Message.ERROR_ADD_USER_ROLE;
                        return message;
                    }
                }
            }
            else
            {
                message = Message.NOT_EXISTS_USER;
            }

            return message;

            #endregion Business Logic
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(LoginResponse, string)> Login(LoginRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var response = new LoginResponse();

            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                message = Message.NOT_ENOUGH_INFO;
                return (response, message);
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _unitOfWork.UserRepository.FindSingle
            (
                expression: x => x.UserName == request.UserName && x.Status != Status.Deleted,
                includes: q => q.Include(x => x.Roles).Include(x => x.UserTokens)
            );
            var isValid = _unitOfWork.UserRepository.CheckPassword(user, request.Password);

            if (user == null || !isValid)
            {
                message = Message.LOGIN_FAILED;
                return (response, message);
            }

            if (user.IsLocked)
            {
                message = Message.ACCOUNT_LOCKED;
                return (response, message);
            }

            if (!user.EmailConfirmed)
            {
                message = Message.EMAIL_NOT_YET_CONFIRMED;
                return (response, message);
            }

            var roles = _unitOfWork.UserRepository.GetRoles(user);
            var accessToken = JwtHelper.GenerateAccessToken(user.Email, user.Id, user.UserName, _appSettings.JwtSettings.Issuer, _appSettings.JwtSettings.Audience, _appSettings.JwtSettings.Secret, roles);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            if (string.IsNullOrEmpty(accessToken.Item1) || string.IsNullOrEmpty(refreshToken.Item1))
            {
                message = Message.LOGIN_FAILED;
                return (response, message);
            }

            User result;

            if (user.UserTokens.Count == 0)
            {
                var listToken = new List<UserToken>()
                {
                    new UserToken
                    {
                        Type = "ACT",
                        Token = accessToken.Item1,
                        UserId = user.Id,
                        ExpiredTime = accessToken.Item2
                    },
                    new UserToken
                    {
                        Type = "RFT",
                        Token = refreshToken.Item1,
                        UserId = user.Id,
                        ExpiredTime = refreshToken.Item2
                    }
                };

                user.UserTokens.AddRange(listToken);
                result = await _unitOfWork.UserRepository.UpdateAsync(user);
            }
            else
            {
                var userAccessToken = user.UserTokens.FirstOrDefault(x => x.Type == "ACT");
                var userRefreshToken = user.UserTokens.FirstOrDefault(x => x.Type == "RFT");

                if (userAccessToken == null || userRefreshToken == null)
                {
                    message = Message.LOGIN_FAILED;
                    return (response, message);
                }

                userAccessToken.Token = accessToken.Item1;
                userAccessToken.ExpiredTime = accessToken.Item2;
                userRefreshToken.Token = refreshToken.Item1;
                userRefreshToken.ExpiredTime = refreshToken.Item2;
                result = await _unitOfWork.UserRepository.UpdateAsync(user);
            }

            response.UserName = user.UserName;
            response.AccessToken = accessToken.Item1;
            response.RefreshToken = refreshToken.Item1;

            return (response, message);

            #endregion Business Logic
        }

        /// <summary>
        /// Register an account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> Register(RegisterRequest request)
        {
            #region Verify Request

            var message = string.Empty;

            if (request == null || string.IsNullOrEmpty(request?.UserName) || string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.Email))
            {
                message = Message.NOT_ENOUGH_INFO;
                return message;
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Message.INVALID_EMAIL;
                return message;
            }

            var canConnectDb = await _unitOfWork.Context.Database.CanConnectAsync();

            if (!canConnectDb)
            {
                message = Message.CANNOT_CONNECT_DB;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };

            using (var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isUserCreated = await _unitOfWork.UserRepository.CreateAsync(user, request.Password);

                    if (!isUserCreated)
                    {
                        message = Message.ERROR_CREATE_USER;
                        return message;
                    }

                    var roleCode = RoleInfoAttribute.ToCode(Foundation.Common.Constants.Constants.RoleType.Customer);
                    var role = await _unitOfWork.RoleRepository.FindSingle(x => x.Code == roleCode && x.Status != Status.Deleted);
                    var isRoleAdded = await _unitOfWork.UserRepository.AddToRoleAsync(user, role);

                    if (!isRoleAdded)
                    {
                        message = Message.ERROR_CREATE_USER;
                        return message;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    message = ex.Message;
                    return message;
                }
            }

            return message;

            #endregion Business Logic
        }
    }
}