﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.Models;
using OpusLink.Entity;
using OpusLink.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OpusLink.Entity.DTO.AccountDTO.SendEmail;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;

namespace OpusLink.Service.AccountServices
{
    public interface IAccountService
    {
        Task<ApiResponseModel> Login(LoginDTO model);
        Task<ApiResponseModel> ConfirmEmail(string token, string email);
    }
    public class AccountService : IAccountService
    {
        private UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTSetting _jwtSetting;
        private readonly OpusLinkDBContext _dbContext;

        public AccountService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IOptionsMonitor<JWTSetting> jwtSetting, OpusLinkDBContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSetting = jwtSetting.CurrentValue;
            _dbContext = dbContext;
        }

        public async Task<ApiResponseModel> Login(LoginDTO model)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var result = await ValidLogin(model);
            if (!result.IsSuccess)
            {
                return result;
            }

            // Lấy thông tin user về
            var claims = await GetClaimsUsers(model);

            //Mô tả Token trả về bao gồm những thông tin nào
            var tokenDecription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3), //Security Key này tồn tại được trong bao nhiêu giờ
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Issuer,

                //Mã chữ ký - Dùng thuật toán nào để mã hóa
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };

            //Tạo token dựa vào mô tả của TokenDecription
            var token = jwtTokenHandler.CreateToken(tokenDecription);
            string accessToken = jwtTokenHandler.WriteToken(token);
            if (!result.IsSuccess)
            {
                return result;
            }

            //Trả về cái accessToken
            result.Data = accessToken;
            return result;
        }

        //Confirm Email khi ấn link từ Email
        public async Task<ApiResponseModel> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return new ApiResponseModel
                    {
                        Code = 200,
                        IsSuccess = true,
                        Message = "Confirm email successfully"
                    };
                }
            }
            return new ApiResponseModel
            {
                Code = 400,
                IsSuccess = false,
                Message = "Confirm email failed"
            };
        }

        //Lấy về thông tin của người dùng bao gồm, Name, Email, Id,..
        private async Task<List<Claim>> GetClaimsUsers(LoginDTO model)
        {
            List<Claim> result;
            var user = await _userManager.FindByNameAsync(model.UserName);

            //1 user có thể có nhiều roles
            var roles = await _userManager.GetRolesAsync(user);
            string role = roles[0].ToString();
            result = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id.ToString()),
                new(ClaimTypes.Role, role),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            return result;
        }

        //Check xem User có tồn tại hay không
        private async Task<ApiResponseModel> ValidLogin(LoginDTO user)
        {
            var result = new ApiResponseModel()
            {
                Code = 200,
                Message = "Ok",
                IsSuccess = true
            };
            var userIdentity = await _userManager.FindByNameAsync(user.UserName);
            if (userIdentity == null || !await _userManager.CheckPasswordAsync(userIdentity, user.Password))
            {
                return new ApiResponseModel
                {
                    Code = 400,
                    IsSuccess = false,
                    Message = "Username or Password is incorrect!"
                };
            }
            return result;
        }
    }
}
