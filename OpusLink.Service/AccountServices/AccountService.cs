using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.Models;
using OpusLink.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpusLink.Service.AccountServices
{
    public interface IAccountService
    {
        Task<ApiResponseModel> LoginUser(LoginDTO model);
        Task<ApiResponseModel> LoginAdmin(LoginDTO model);
        Task<ApiResponseModel> ConfirmEmail(string token, string email);
        Task<ApiResponseModel> UpdateUserRole(UpdateRoleDTO model);
        string GenerateRandomString();
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

        public async Task<ApiResponseModel> LoginUser(LoginDTO model)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var result = await ValidLoginUser(model);
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
/*            if (!result.IsSuccess)
            {
                return result;
            }*/

            //Trả về cái accessToken
            result.Data = accessToken;
            return result;
        }

        public async Task<ApiResponseModel> LoginAdmin(LoginDTO model)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var result = await ValidLoginAdmin(model);
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
            /*            if (!result.IsSuccess)
                        {
                            return result;
                        }*/

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
                new("userName", user.UserName),
                new("email", user.Email),
                new("userId", user.Id.ToString()),
                new(ClaimTypes.Role, role),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            return result;
        }

        private async Task<List<Claim>> GetClaimsUsersChange(UpdateRoleDTO model)
        {
            List<Claim> result;
            string role;
            var user = await _userManager.FindByNameAsync(model.UserName);

            //1 user có thể có nhiều roles
            var roles = await _userManager.GetRolesAsync(user);
            if(model.CurrentRole == "Freelancer")
            {
                role = roles[1].ToString();
            }
            else
            {
                role = roles[0].ToString();
            }
                
            result = new List<Claim>()
            {
                new("userName", user.UserName),
                new("email", user.Email),
                new("userId", user.Id.ToString()),
                new(ClaimTypes.Role, role),
                new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            return result;
        }

        public async Task<ApiResponseModel> UpdateUserRole(UpdateRoleDTO model)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSetting.Key);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new ApiResponseModel { IsSuccess = false, Message = "User not found" };
            }

            // Lấy thông tin user về
            var claims = await GetClaimsUsersChange(model);

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

            //Trả về cái accessToken
            return new ApiResponseModel() { Code =200, Message = "Success", IsSuccess = true, Data = accessToken };
        }

        //Check xem User có tồn tại hay không
        private async Task<ApiResponseModel> ValidLoginUser(LoginDTO user)
        {
            var result = new ApiResponseModel()
            {
                Code = 200,
                Message = "ValidLogin thành công",
                IsSuccess = true
            };
            try
            {
                var userIdentity = await _userManager.FindByNameAsync(user.UserName);
                if (userIdentity == null || !await _userManager.CheckPasswordAsync(userIdentity, user.Password))
                {
                    return new ApiResponseModel
                    {
                        Code = 400,
                        IsSuccess = false,
                        Message = TotalMessage.LoginError
                    };
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(userIdentity);
                    bool isAdmin = userRoles.Any(role => role == "Admin");

                    if (isAdmin)
                    {
                        return new ApiResponseModel
                        {
                            Code = 403, // Forbidden status code
                            IsSuccess = false,
                            Message = TotalMessage.LogingErrorRoleAdmin
                        };
                    }

                    /*if(!userIdentity.EmailConfirmed)
                    {
                        return new ApiResponseModel
                        {
                            Code = 400,
                            IsSuccess = false,
                            Message = TotalMessage.LoginErrorEmailConfirm
                        };
                    }*/

                    if (userIdentity.Status == 0)
                        if (userIdentity.EndBanDate > DateTime.Now)
                        {
                            return new ApiResponseModel
                            {
                                Code = 400,
                                IsSuccess = false,
                                Message = TotalMessage.LoginFailCuzBanUser + userIdentity.EndBanDate?.ToString("dd/MM/yyyy") + " .Vui lòng liên hệ với admin để được hỗ trợ. Liên hệ support@opuslink.com"
                            };
                        }
                        else
                        {
                            userIdentity.Status = 1;
                            await _userManager.UpdateAsync(userIdentity);
                        }
                }

            }catch(Exception ex)
            {
                return new ApiResponseModel
                {
                    Code = 400,
                    IsSuccess = false,
                    Message = TotalMessage.LoginError
                };
            }
            return result;
        }

        //Check xem User có tồn tại hay không
        private async Task<ApiResponseModel> ValidLoginAdmin(LoginDTO user)
        {
            var result = new ApiResponseModel()
            {
                Code = 200,
                Message = "ValidLogin thành công",
                IsSuccess = true
            };
            try
            {
                var userIdentity = await _userManager.FindByNameAsync(user.UserName);
                if (userIdentity == null || !await _userManager.CheckPasswordAsync(userIdentity, user.Password))
                {
                    return new ApiResponseModel
                    {
                        Code = 400,
                        IsSuccess = false,
                        Message = TotalMessage.LoginError
                    };
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(userIdentity);
                    bool isAdmin = userRoles.Any(role => role == "Admin");

                    if (!isAdmin)
                    {
                        return new ApiResponseModel
                        {
                            Code = 403, // Forbidden status code
                            IsSuccess = false,
                            Message = TotalMessage.LoginErrorRoleUser
                        };
                    }

                    if (userIdentity.Status == 0)
                        if (userIdentity.EndBanDate > DateTime.Now)
                        {
                            return new ApiResponseModel
                            {
                                Code = 400,
                                IsSuccess = false,
                                Message = TotalMessage.LoginFailCuzBanUser + userIdentity.EndBanDate?.ToString("dd/MM/yyyy") + " .Vui lòng liên hệ với admin để được hỗ trợ. Liên hệ support@opuslink.com"
                            };
                        }
                        else
                        {
                            userIdentity.Status = 1;
                            await _userManager.UpdateAsync(userIdentity);
                        }
                }

            }
            catch (Exception ex)
            {
                return new ApiResponseModel
                {
                    Code = 400,
                    IsSuccess = false,
                    Message = TotalMessage.LoginError
                };
            }
            return result;
        }

        public string GenerateRandomString()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            // Thêm ít nhất một ký tự in thường, in hoa, số, và ký tự đặc biệt
            sb.Append((char)random.Next('a', 'z' + 1)); // Thêm ký tự in thường
            sb.Append((char)random.Next('A', 'Z' + 1)); // Thêm ký tự in hoa
            sb.Append((char)random.Next('0', '9' + 1)); // Thêm ký tự số
            sb.Append((char)random.Next(33, 48)); // Thêm ký tự đặc biệt từ ASCII 33 đến 47

            // Thêm các ký tự ngẫu nhiên cho đến khi đạt được độ dài tối thiểu 6 ký tự
            while (sb.Length < 6)
            {
                char nextChar = (char)random.Next(33, 126); // Ký tự từ ASCII 33 đến 125 (bao gồm ký tự đặc biệt và chữ cái)
                sb.Append(nextChar);
            }

            // Trộn ngẫu nhiên chuỗi
            return RandomizeString(sb.ToString());
        }

        private string RandomizeString(string input)
        {
            char[] chars = input.ToCharArray();
            Random random = new Random();
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = random.Next(i, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
            return new string(chars);
        }
    }
}
