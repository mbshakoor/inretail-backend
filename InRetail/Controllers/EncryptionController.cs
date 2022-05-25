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
using Microsoft.Extensions.Configuration;
using InRetailCore.Ecryption;
using InRetailDAL.ConstFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace InRetail.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EncryptionController : ControllerBase
    {
        private readonly IUserService _userService;
        public EncryptionController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("EncryptData")]
        public async Task<ActionResult<string>> EncryptData(string Message)
        {
            string error = "", PlainText = "", Username = "", Password = "";
            int length = Message.Split(',').Length;
            if (length != 3)
                error = "Invalid Scheme";

            if (length == 3)
            {
                PlainText = Message.Split(',')[0].Trim();
                Username = Message.Split(',')[1].Trim();
                Password = Message.Split(',')[2].Trim();

                string username = EncryptionConsts.UserName;
                string password = EncryptionConsts.Password;
                if (!username.Equals(Username) || !password.Equals(Password))
                    error = "Invalid Data";
            }
            if(error.Length > 0)
                return NotFound(error);

            string result = AESEncryption.EncryptData(PlainText);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("HandShake")]
        public async Task<ActionResult<string>> EncryptionHandShake(string Message)
        {
            if (Message.ToLower() == ConstHelper.chaveAlpor.ToLower())
            {
                _userService.GetEncCredentials();
                string username = EncryptionConsts.UserName;
                string password = EncryptionConsts.Password;
                Message = username + ", " + password;
            }
            
            return Ok(Message);
        }

    }
}
