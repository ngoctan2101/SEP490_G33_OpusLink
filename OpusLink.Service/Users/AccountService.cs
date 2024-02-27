using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpusLink.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using OpusLink.Entity.Models;
using OpusLink.Entity.DTO.HaiDTO;
using OpusLink.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace OpusLink.Service.Users
{
    public interface IAccountService
    {
        Task<ApiResponseModel> Login(LoginDTO model);
        Task<ApiResponseModel> Register(RegisterDTO model);
        //List<Account>? GetAccountsByKeyword(string keyword);
        //List<Account>? GetAllAccount();
        //Account? GetAccountById(int id);
        //Account? GetAccountByEmailAndPassword(string email, string password);
        //Account? GetAccountByEmail(string email);
        //CreateAccountResponse AddAccount(CreateAccountRequest account);
        //UpdateAccountResponse UpdateAccount(UpdateAccountRequest account);
        //bool UpdateDeleteStatusAccount(int id);
        //List<Account> GetAccountsByRoleId(int roleId);
    }
    public class AccountService : IAccountService
    {
        private UserManager<OpusLink.Entity.Models.User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<OpusLink.Entity.Models.User> _signInManager;
        private readonly JWTSetting _jwtSetting;
        private readonly OpusLinkDBContext _dbContext;

        public AccountService(UserManager<OpusLink.Entity.Models.User> userManager, RoleManager<Role> roleManager, SignInManager<OpusLink.Entity.Models.User> signInManager, IOptionsMonitor<JWTSetting> jwtSetting, OpusLinkDBContext dbContext)
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

        public async Task<ApiResponseModel> Register(RegisterDTO model)
        {
            ApiResponseModel result = new ApiResponseModel()
            {
                Code = 200,
                IsSuccess = true,
                Message = "User created success",
                Data = model
            };

            //( Đoạn này : Nếu Mail và Name đã tồn tại
            var userExistMail = await _userManager.FindByEmailAsync(model.Email);
            var userExistName = await _userManager.FindByNameAsync(model.UserName);
            if (userExistMail != null || userExistName != null)
            {
                return new ApiResponseModel()
                {
                    Code = 400,
                    Message = "User has been already existed!",
                    IsSuccess = false
                };
            }
            //)

            //(Đoạn này : Nếu không tồn tại thì tạo ra 1 user
            //ID của User thì nó tự tạo rồi
            var user = new OpusLink.Entity.Models.User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
            };
            //)

            var resultCreateUser = await _userManager.CreateAsync(user, model.Password);

            //Role mặc định là Freelancer and Employer
            var resultRoleFreelancer = await _userManager.AddToRoleAsync(user, Roles.Freelancer.ToString());
            var resultRoleEmployer = await _userManager.AddToRoleAsync(user, Roles.Employer.ToString());

            if (!resultCreateUser.Succeeded)
            {
                return new ApiResponseModel()
                {
                    Code = 400,
                    Message = "Error when create user",
                    IsSuccess = false
                };
            }/*
            else
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var email_body = $"Please confirm your account by <a href='{_jwtSetting.Issuer}/Account/ConfirmEmail?email={user.Email}&code={code}'>clicking here</a>";
                var callback_url = Request.Scheme + "://" + Request.Host + Url.Ac
            }*/
            return result;

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

        //private readonly FstoreContext _context;
        //private readonly IEncryptionService _encryptionService;

        //public AccountService(FstoreContext context, IEncryptionService encryptionService)
        //{
        //    _context = context;
        //    _encryptionService = encryptionService;
        //}


        //public List<Account> GetAccountsByKeyword(string keyword)
        //{
        //    try
        //    {
        //        var accounts = _context.Accounts.Where(x => x.FullName.ToLower().Contains(keyword.ToLower())).ToList();
        //        return accounts;

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

    }
}
