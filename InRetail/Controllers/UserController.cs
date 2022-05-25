using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InRetailDAL.Dtos;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using InRetailDAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using InRetailDAL.ConstFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;

namespace InRetail.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getAllUsers")]
        public async Task<UserModelResponse> GetAllUsers(int BranchId)
        {
            UserModelResponse response = new UserModelResponse();
            try
            {
                var result = await _userService.GetAllUserAsync(BranchId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.UserModel = JsonConvert.DeserializeObject<List<UserModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getUserById")]
        public async Task<ActionResult<UserUpdateDto>> GetUserById(int Id)
        {
            UserUpdateDto response = new UserUpdateDto();
            try
            {
                var result = await _userService.GetUserByIdAsync(Id);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_FOUND;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<UserUpdateDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<ResponseDto>> CreateUser( UserInsertDto user)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Users user1 = _mapper.Map<Users>(user);
                var result = await _userService.AddUserAsync(user1);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_FOUND;
                if(result.BranchId == ConstHelper.INVALID_BRANCH)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_BRANCH;
                if (result.Id == ConstHelper.USER_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.USER_ALREADY_EXIST;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("updateUser")]
        public async Task<ActionResult<ResponseDto>> UpdateUser( UserUpdateObj user)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Users user1 = _mapper.Map<Users>(user);
                var result = await _userService.UpdateUserAsync(user1);

                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_UPDATE;
                if (result.BranchId == ConstHelper.INVALID_BRANCH)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_BRANCH;
                if (result.Id == ConstHelper.USER_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.USER_ALREADY_EXIST;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("searchUsers")]
        public async Task<UserModelResponse> SearchUsers(int? BranchId, DateTime? FromDate, DateTime? ToDate, string? ContactNo, string? Username)
        {
            UserModelResponse response = new UserModelResponse();
            try
            {
                FromDate = ConstHelper.GetFromDate(FromDate);
                ToDate = ConstHelper.GetToDate(ToDate);

                var result = await _userService.SearchUsersAsync(BranchId, FromDate, ToDate, ContactNo, Username);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.UserModel = JsonConvert.DeserializeObject<List<UserModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("registerUserDevice")]
        public async Task<UserResponseDto> RegisterUserDevice(RegisterUserDeviceDto device)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                var result = await _userService.RegisterUserDeviceAsync(device.Username, device.Password, device.OrgnanizationNo, device.IMEI, ipAddress());
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_LIST;
                if (result.ErrorMessage == ErrorHelper.IMEI_INVALID)
                    response = result;
                if (result.OrganizationId == ConstHelper.INVALID_ORGANIZATION)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_ORGANIZATION;
                if (result.Id == ConstHelper.INVALID_USER)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_CREDENTIALS;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = result;
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    setTokenCookie(result.RefreshToken);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpGet("loginUser")]
        public async Task<UserResponseDto> LoginUser(string username, string password, int OrganizationId)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                var result = await _userService.LoginUserAsync(username, password, OrganizationId, ipAddress());
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_USER_LOGIN;
                if (result.Id == ConstHelper.INVALID_USER)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_CREDENTIALS;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = result;
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    setTokenCookie(result.RefreshToken);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("authenticateUser")]
        public async Task<ResponseDto> AuthenticateUser(int UserId, string password)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var result = await _userService.AuthenticateUserAsync(UserId, password);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.INCORRECT_PASSWORD;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpGet("isIMEIRegistered")]
        public async Task<DeviceRegisterationDto> IsIMEIRegistered(string IMEI)
        {
            DeviceRegisterationDto response = new DeviceRegisterationDto();
            try
            {
                var result = _userService.IsIMEIRegisterd(IMEI);
                if (result == null)
                {
                    result = new DeviceRegistration();
                    result.OrganizationId = -1;
                    response.ErrorMessage = ErrorHelper.IMEI_NOT_REGISTERED;
                }
                else
                {
                    response = _mapper.Map<DeviceRegisterationDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }

            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("resetUserPassword")]
        public async Task<ResponseDto> ResetUserPassword(UserResetPasswordDto user)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Users user1 = _mapper.Map<Users>(user);
                var result = await _userService.ResetPassword(user1);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_PWD_RESET;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("changeUserPassword")]
        public async Task<ResponseDto> ChangeUserPassword(UserChangePasswordDto user)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Users user1 = _mapper.Map<Users>(user);
                var result = await _userService.ChangePassword(user1, user.NewPassword);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_PWD_CHANGE;
                if (result.Id == ConstHelper.PWD_MISMATCHED)
                    response.ErrorMessage = ErrorHelper.ERROR_PWD_MISMATCHED;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("changeUserPasswordAfterOTP")]
        public async Task<ResponseDto> ChangeUserPasswordAfterOTP(UserIMEIChangePasswordDto user)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Users user1 = _mapper.Map<Users>(user);
                var result = await _userService.ChangePassword(user1, user.NewPassword);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_PWD_CHANGE;
                if (result.Id == ConstHelper.PWD_MISMATCHED)
                    response.ErrorMessage = ErrorHelper.ERROR_PWD_MISMATCHED;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    _userService.UpdateIMEIRegisteredStatus(user.IMEI);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public UserResponseDto RefreshToken(RevokeTokenDto model)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                //var refreshToken = Request.Cookies[ConstHelper.TokenTag];
                var refreshToken = model.Token ?? RequestRefreshToken();
                if (string.IsNullOrEmpty(refreshToken))
                {
                    response.ErrorMessage = ConstHelper.ERR_EMPTY_TOKEN;
                }
                else
                {
                    response = _userService.RefreshToken(refreshToken, ipAddress());

                    if (response == null)
                    {
                        response.ErrorMessage = ConstHelper.ERR_INVALID_TOKEN;
                    }
                    else
                    {
                        response.ErrorMessage = ErrorHelper.SUCCESS;
                        setTokenCookie(response.RefreshToken);
                    }
                }
                return response;
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        public UserResponseDto RevokeToken(RevokeTokenDto model)
        {
            UserResponseDto response = new UserResponseDto();
            try
            {
                // accept token from request body or cookie
                //var token = model.Token ?? Request.Cookies[ConstHelper.TokenTag];
                var token = model.Token ?? RequestRefreshToken();
                if (string.IsNullOrEmpty(token))
                {
                    response.ErrorMessage = ConstHelper.ERR_EMPTY_TOKEN;
                }
                else
                {
                    var result = _userService.RevokeToken(token, ipAddress());
                    if (!result)
                        response.ErrorMessage = ConstHelper.ERR_TOKEN_NOT_FOUND;
                    else
                        response.ErrorMessage = ConstHelper.ERR_TOKEN_REVOKED;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append(ConstHelper.TokenTag, token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey(ConstHelper.IpTag))
                return Request.Headers[ConstHelper.IpTag];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private string RequestRefreshToken()
        {
            string refreshToken = "";
            if (Request.Headers.ContainsKey(ConstHelper.TokenTag))
                refreshToken = Request.Headers[ConstHelper.TokenTag];
            return refreshToken;
        }

        //[AllowAnonymous]
        [HttpGet("verifyUserOTP")]
        public async Task<bool> VerifyUserOTP(int UserId, string OTP)
        {
            var result = await _userService.VerifyUserOTP(UserId, OTP);
            return result;
        }
    }
}
