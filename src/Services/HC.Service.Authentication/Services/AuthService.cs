using AutoMapper;
using HC.Foundation.Cormmon.Attributes;
using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Models.DTOs;
using HC.Service.Authentication.Models.Requests;
using HC.Service.Authentication.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace HC.Service.Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Register an account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> Register(RegisterRequest request)
        {
            var message = string.Empty;

            #region Verify Request

            if (request == null || string.IsNullOrEmpty(request?.UserName) || string.IsNullOrEmpty(request?.Password) || string.IsNullOrEmpty(request?.Email))
            {
                message = Constants.Message.NOT_ENOUGH_INFO;
                return message;
            }

            var registerDTO = _mapper.Map<RegisterDTO>(request);

            if (registerDTO == null)
            {
                message = Constants.Message.NOT_ENOUGH_INFO;
                return message;
            }

            if (!IsValidEmail(registerDTO.Email))
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
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                Address = registerDTO.Address
            };            

            try
            {
                var roleCode = RoleInfoAttribute.ToCode(Foundation.Core.Constants.Constants.Role.Customer);
                var role = await _unitOfWork.RoleRepository.FindSingle(x => x.Code == roleCode);
                //user.Roles.Add(role);

                var isUserCreated = await _unitOfWork.UserRepository.CreateAsync(user, registerDTO.Password);

                if (!isUserCreated)
                {
                    message = Constants.Message.ERROR_CREATE_USER;
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
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }

            #endregion Business Logic

            return message;
        }

        /// <summary>
        /// Email Validation
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}