using InRetailCore.Ecryption;
using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Dtos;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.ServiceImp
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly InRetailContext _context;

        public UserService(IUserRepository userRepository, IBranchRepository branchRepository, InRetailContext context)
        {
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _context = context;
        }

        public async Task<string> GetAllUserAsync(int BranchId)
        {
            return await _userRepository.GetAllUserAsync(BranchId);
        }

        public async Task<Users> GetUserByIdAsync(int Id)
        {
            return await _userRepository.GetUserByIdAsync(Id);
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            Users response = new Users();
            var branch = await _branchRepository.GetBranchByIdAsync(user.BranchId);
            if (branch == null)
            {
                response.BranchId = ConstHelper.INVALID_BRANCH;
            }
            else
            {
                var tempUser = _userRepository.GetUserByNameAsync(user.Username, user.BranchId);
                if (tempUser != null)
                {
                    response.Id = ConstHelper.USER_ALREADY_EXIST;
                }
                else
                {
                    user.CreatedOn = DateTime.Now;
                    user.IsActive = true;
                    user.UpdatedOn = DateTime.Now;
                    user.CreatedBy = user.CreatedBy == 0 ? ConstHelper.APP_ADMIN_USER_ID : user.CreatedBy;

                    response = await _userRepository.AddAsync(user);
                }
            }
            return response;
        }

        public async Task<Users> AddAdminUserAsync(int BranchId, string ContactNo)
        {
            Users user = new Users();
            user.CreatedOn = DateTime.Now;
            user.IsActive = true;
            user.UpdatedOn = DateTime.Now;
            user.BranchId = BranchId;
            user.ContactNo = ContactNo;
            user.LoginId = ConstHelper.ADMIN_USER_ID;
            user.Username = ConstHelper.ADMIN_USER;
            user.Password = GeneralHelper.GetRandomPassword();
            user.CreatedBy = user.CreatedBy == 0 ? ConstHelper.APP_ADMIN_USER_ID : user.CreatedBy;

            return await _userRepository.AddAsync(user);
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            Users response = new Users();
            var branch = await _branchRepository.GetBranchByIdAsync(user.BranchId);
            if (branch == null)
            {
                response.BranchId = ConstHelper.INVALID_BRANCH;
            }
            else
            {
                var tempUser1 = _userRepository.GetUserByNameAndIdAsync(user.Username, user.BranchId, user.Id);
                if (tempUser1 != null)
                {
                    response.Id = ConstHelper.USER_ALREADY_EXIST;
                }
                else
                {
                    Users tempUser = await _userRepository.GetUserByIdAsync(user.Id);
                    tempUser.ContactNo = user.ContactNo;
                    tempUser.LoginId = user.LoginId;
                    tempUser.Username = user.Username;
                    tempUser.UpdatedOn = DateTime.Now;
                    tempUser.UpdatedBy = user.UpdatedBy == 0 ? ConstHelper.APP_ADMIN_USER_ID : user.UpdatedBy;

                    response = await _userRepository.UpdateAsync(tempUser);
                }
            }
            return response;
        }

        public async Task<UserResponseDto> RegisterUserDeviceAsync(string username, string password, string OrgnanizationNo, string IMEI, string ipAddress)
        {
            UserResponseDto response = new UserResponseDto();
            var organization = _context.Organizations.SingleOrDefault(x => x.Code == OrgnanizationNo);
            if (string.IsNullOrEmpty(IMEI))
            {
                response.ErrorMessage = ErrorHelper.IMEI_INVALID;
                return response;
            }
            if (organization == null)
            {
                response.OrganizationId = ConstHelper.INVALID_ORGANIZATION;
            }
            else
            {
                response = await LoginUserAsync(username, password, organization.Id, ipAddress);
                if (response.Id != ConstHelper.INVALID_USER)
                {
                    var device = _context.DeviceRegistrations.SingleOrDefault(x => x.IMEI == IMEI);
                    if (device == null)
                    {
                        DeviceRegistration deviceRegistration = new DeviceRegistration();
                        deviceRegistration.OrganizationId = organization.Id;
                        deviceRegistration.IMEI = IMEI;
                        deviceRegistration.CreatedOn = DateTime.Now;
                        deviceRegistration.IsPasswordChanged = false;
                        _context.DeviceRegistrations.Add(deviceRegistration);
                        _context.SaveChanges();
                    }

                    var otp = await _userRepository.CreateUserOTP(response.Id);
                    response.Code = otp.Code;
                }
            }
            return response;
        }

        public async Task<UserResponseDto> LoginUserAsync(string username, string password, int OrganizationId, string ipAddress)
        {
            UserResponseDto response = new UserResponseDto();
            var user = _userRepository.LoginUserAsync(username, password, OrganizationId);
            // return null if user not found
            if (user == null)
            {
                response.Id = ConstHelper.INVALID_USER;
            }
            else
            {
                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(user);
                var refreshToken = generateRefreshToken(ipAddress);
                refreshToken.UserId = user.Id;

                // save refresh token
                _context.RefreshTokens.Add(refreshToken);
                _context.SaveChanges();

                response = new UserResponseDto(user.Id, user.Username, user.BranchId, jwtToken, refreshToken.Token);
            }
            return response;
        }

        public async Task<Users> AuthenticateUserAsync(int UserId, string password)
        {
            return await _userRepository.AuthenticateUserAsync(UserId, password);
        }


        public async Task<Users> ResetPassword(Users user)
        {
            Users tempUser = await _userRepository.GetUserByIdAsync(user.Id);
            tempUser.Password = user.Password;
            tempUser.UpdatedOn = DateTime.Now;
            tempUser.UpdatedBy = ConstHelper.RESET_USER_ID;

            return await _userRepository.UpdateAsync(tempUser);
        }

        public async Task<Users> ChangePassword(Users user, string newpassword)
        {
            Users tempUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (tempUser.Password == user.Password)
            {
                tempUser.Password = newpassword;
                tempUser.UpdatedOn = DateTime.Now;
                tempUser.UpdatedBy = ConstHelper.RESET_USER_ID;
                tempUser = await _userRepository.UpdateAsync(tempUser);
            }
            else
            {
                tempUser.Id = ConstHelper.PWD_MISMATCHED; // if both passwords are mismatched
            }
            return tempUser;
        }

        public async Task<string> SearchUsersAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, string? ContactNo, string? Username)
        {
            return await _userRepository.SearchUsersAsync(BranchId, FromDate, ToDate, ContactNo, Username);
        }

        public UserResponseDto RefreshToken(string token, string ipAddress)
        {
            var refreshToken = _context.RefreshTokens.SingleOrDefault(x => x.Token == token);
            var user = _context.Users.SingleOrDefault(u => u.Id == refreshToken.UserId);

            // return null if no user found with token
            if (user == null) return null;

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            newRefreshToken.UserId = user.Id;
            refreshToken.RevokedOn = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            _context.RefreshTokens.Add(newRefreshToken);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new UserResponseDto(user.Id, user.Username, user.BranchId, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var refreshToken = _context.RefreshTokens.SingleOrDefault(x => x.Token == token);

            // return false if no user found with token
            if (refreshToken == null) return false;


            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.RevokedOn = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> VerifyUserOTP(int UserId, string OTP)
        {
            return await _userRepository.VerifyUserOTP(UserId, OTP);
        }

        private string generateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(EncryptionConsts.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiredOn = DateTime.UtcNow.AddDays(7),
                    CreatedOn = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        public async void GetEncCredentials()
        {
            var credentials = _context.EncCedentials.FirstOrDefault();

            EncryptionConsts.EncryptionKey = credentials.EncryptionKey;
            EncryptionConsts.Secret = credentials.TokenKey;
            EncryptionConsts.UserName = credentials.UserName;
            EncryptionConsts.Password = credentials.Password;
        }

        public DeviceRegistration IsIMEIRegisterd(string IMEI)
        {
            var device = _context.DeviceRegistrations.SingleOrDefault(x => x.IMEI == IMEI && x.IsPasswordChanged == true);

            return device;
        }

        public bool UpdateIMEIRegisteredStatus(string IMEI)
        {
            var device = _context.DeviceRegistrations.SingleOrDefault(x => x.IMEI == IMEI);
            device.IsPasswordChanged = true;
            _context.Entry(device).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return true; ;
        }

    }
}
