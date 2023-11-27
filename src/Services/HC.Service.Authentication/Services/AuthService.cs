using AutoMapper;
using HC.Foundation.Cormmon.Attributes;
using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Helpers;
using HC.Service.Authentication.Models.Requests;
using HC.Service.Authentication.Models.Responses;
using HC.Service.Authentication.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace HC.Service.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Assign role to User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<string> AssignRole(string email, string roleName)
        {
            #region Verify Request

            var message = string.Empty;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                message = Constants.Message.NOT_ENOUGH_INFO;
                return message;
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _unitOfWork.UserRepository.FindSingle
            (
                expression: x => x.Email == email,
                includes: q => q.Include(x => x.Roles)
            );

            if (user != null)
            {
                if (!await _unitOfWork.RoleRepository.IsExists(x => x.Name == roleName))
                {
                    message = Constants.Message.NOT_EXISTS_ROLE;
                    return message;
                }

                var role = await _unitOfWork.RoleRepository.FindSingle(x => x.Name == roleName);

                if (role != null)
                {
                    if (user.Roles.Contains(role))
                    {
                        message = Constants.Message.GRANTED_ROLE;
                        return message;
                    }

                    var isRoleAdded = await _unitOfWork.UserRepository.AddToRoleAsync(user, role);

                    if (!isRoleAdded)
                    {
                        message = Constants.Message.ERROR_ADD_USER_ROLE;
                        return message;
                    }

                    var isSaved = await _unitOfWork.SaveChangesAsync();

                    if (!isSaved)
                    {
                        message = Constants.Message.ERROR_SAVE;
                        return message;
                    }
                }
            }
            else
            {
                message = Constants.Message.NOT_EXISTS_USER;
            }

            return message;

            #endregion Business Logic
        }

        public async Task<(LoginResponse, string)> Login(LoginRequest request)
        {
            #region Verify Request

            var message = string.Empty;
            var response = new LoginResponse();

            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                message = Constants.Message.NOT_ENOUGH_INFO;
                return (response, message);
            }

            #endregion Verify Request

            #region Business Logic

            var user = await _unitOfWork.UserRepository.FindSingle
            (
                expression: x => x.UserName == request.UserName,
                includes: q => q.Include(x => x.Roles).Include(x => x.UserTokens)
            );
            var isValid = _unitOfWork.UserRepository.CheckPassword(user, request.Password);

            if (user == null || !isValid)
            {
                message = Constants.Message.LOGIN_FAILED;
                return (response, message);
            }

            var roles = _unitOfWork.UserRepository.GetRoles(user);
            var accessToken = _jwtTokenGenerator.GenerateToken(user, roles);

            if (string.IsNullOrEmpty(accessToken))
            {
                message = Constants.Message.LOGIN_FAILED;
                return (response, message);
            }

            var userToken = new UserToken();

            if (user.UserTokens.Count == 0)
            {
                userToken = new UserToken 
                { 
                    Type = "ACT",
                    Token = accessToken,
                    UserId = user.Id,
                };

                await _unitOfWork.UserTokenRepository.AddAsync(userToken);
            }
            else
            {
                userToken = await _unitOfWork.UserTokenRepository.FindSingle(x => x.UserId == user.Id && x.Type == "ACT");
                userToken.Token = accessToken;
                var isAdded = await _unitOfWork.UserRepository.AddToUserTokenAsync(user, userToken);
            }

            var isSaved = await _unitOfWork.SaveChangesAsync();

            if (!isSaved)
            {
                message = Constants.Message.ERROR_SAVE;
                return (response, message);
            }

            response.UserName = user.UserName;
            response.AccessToken = accessToken;

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
                message = Constants.Message.NOT_ENOUGH_INFO;
                return message;
            }

            if (!EmailHelper.IsValidEmail(request.Email))
            {
                message = Constants.Message.INVALID_EMAIL;
                return message;
            }

            var canConnectDb = await _unitOfWork.Context.Database.CanConnectAsync();

            if (!canConnectDb)
            {
                message = Constants.Message.CANNOT_CONNECT_DB;
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

            try
            {
                var roleCode = RoleInfoAttribute.ToCode(Foundation.Core.Constants.Constants.Role.Customer);
                var role = await _unitOfWork.RoleRepository.FindSingle(x => x.Code == roleCode && x.Status != Foundation.Core.Constants.Constants.Status.Deleted);

                if (role != null)
                {
                    user.Roles.Add(role);
                }

                var isUserCreated = await _unitOfWork.UserRepository.CreateAsync(user, request.Password);

                if (!isUserCreated || user.Roles.Count == 0)
                {
                    message = Constants.Message.ERROR_CREATE_USER;
                    return message;
                }

                var isSaved = await _unitOfWork.SaveChangesAsync();

                if (!isSaved)
                {
                    message = Constants.Message.ERROR_SAVE;
                    return message;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }

            return message;

            #endregion Business Logic
        }
    }
}