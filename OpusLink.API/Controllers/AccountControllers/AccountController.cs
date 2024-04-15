using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Service.AccountServices;
using OpusLink.Entity.DTO.AccountDTO.SendEmail;
using OpusLink.Entity.Models;
using Microsoft.AspNetCore.Identity;
using OpusLink.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using OpusLink.Shared.Constants;

namespace OpusLink.API.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, IAccountService userService, IEmailService emailService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _accountService = userService;
            _emailService = emailService;
            _signInManager = signInManager;
        }

        [HttpPost("loginUser")]
        public async Task<ApiResponseModel> LoginUser([FromBody] LoginDTO model)
        {
            try
            {
                //ModelState này sẽ check toàn bộ lỗi trong class LoginDTO trong Business Object rồi liệt kê hết ra
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Data = errors,
                        IsSuccess = false,
                        Message = string.Join(";", errors)
                    };
                }
                var result = await _accountService.LoginUser(model);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel()
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpPost("register")]
        public async Task<ApiResponseModel> Register([FromBody] RegisterDTO model)
        {
            try
            {
                ApiResponseModel result = new ApiResponseModel()
                {
                    Code = 200,
                    IsSuccess = true,
                    Message = TotalMessage.RegisterSuccess,
                    Data = model
                };

                // Đoạn này : Nếu Mail và Name đã tồn tại
                var userExistMail = await _userManager.FindByEmailAsync(model.Email);
                var userExistName = await _userManager.FindByNameAsync(model.UserName);
                if (userExistMail != null || userExistName != null)
                {
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = TotalMessage.RegisterAccountExists,
                        IsSuccess = false
                    };
                }

                if((DateTime.Now - model.DateOfBirth).TotalDays / 365 < 18)
                {
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = TotalMessage.RegisterNotEnoughAge,
                        IsSuccess = false
                    };
                }

                //Đoạn này : Nếu không tồn tại thì tạo ra 1 user
                //ID của User thì nó tự tạo rồi
                var user = new User()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Dob = model.DateOfBirth,
                    AmountMoney = 0,
                    Status = 1
                };

                var resultCreateUser = await _userManager.CreateAsync(user, model.Password);
                if(resultCreateUser.Succeeded == false)
                {
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = TotalMessage.RegisterError,
                        IsSuccess = false
                    };
                }

                //Role mặc định là Freelancer and Employer
/*                var resultRoleFreelancer = await _userManager.AddToRoleAsync(user, Roles.Freelancer.ToString());*/
/*                if (resultRoleFreelancer.Succeeded == false)
                {
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = TotalMessage.RegisterError,
                        IsSuccess = false
                    };
                }*/
/*                var resultRoleEmployer = await _userManager.AddToRoleAsync(user, Roles.Employer.ToString());*/

                //Role cho Admin
                var resultRoleEmployer = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                if (!resultCreateUser.Succeeded)
                {
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = TotalMessage.RegisterError,
                        IsSuccess = false
                    };
                }

                SendEmail(user);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet("confirmEmail")]
        public async Task<ApiResponseModel> ConfirmEmail(string token, string email)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Data = errors,
                        IsSuccess = false,
                        Message = string.Join(";", errors)
                    };
                }
                var result = await _accountService.ConfirmEmail(token, email);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet("forgotPassword")]
        [AllowAnonymous]
        public async Task<ApiResponseModel> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string randomString = _accountService.GenerateRandomString();
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string userName = user.UserName;
                var passwordResetLink = UrlConstant.UserClientBaseUrl + "/Account/ResetPassword" + "?token=" + Uri.EscapeDataString(token) + "&email=" + user.Email;
                // Tạo nội dung HTML cho email
                string titleContent = "Đổi mật khẩu - Opuslink";

                string emailContent = "Xin chào " + userName + ",\r\n\r\n" +
                    "Cảm ơn đã sử dụng dịch vụ của Opuslink, nền tảng tìm việc làm freelancer hàng đầu. " +
                    "Nếu bạn đã quên mật khẩu của mình, đừng lo lắng! Chúng tôi sẽ giúp bạn khôi phục mật khẩu một cách dễ dàng.\r\n\r\n" +
                    "Vui lòng nhấp vào liên kết dưới đây để thiết lập lại mật khẩu của bạn :" + "\r\n" +
                    passwordResetLink + "\r\n\r\n" +
                    "Nếu bạn không Đặt lại mật khẩu thì bạn có thể sử dụng mật khẩu này để đăng nhập : " + randomString + "\r\n" +
                    "Lưu ý : Bạn nên đặt lại mật khẩu để đảm bảo sự an toàn" + "\r\n" +
                    "Nếu bạn không thực hiện yêu cầu này, bạn có thể bỏ qua email này.\r\n" +
                    "Cảm ơn bạn đã sử dụng Opuslink. Nếu bạn cần hỗ trợ hoặc có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi qua email support@opuslink.com.\r\n\r\n" +
                    "Trân trọng,\r\nĐội ngũ hỗ trợ Opuslink";
                var message = new MessageEmail(new string[] { user.Email! }, titleContent, emailContent);
                _emailService.SendEmail(message);
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, randomString);

                return new ApiResponseModel()
                {
                    Code = 200,
                    IsSuccess = true,
                    Message = TotalMessage.ForgotPasswordSuccess,
                };
            }
            return new ApiResponseModel()
            {
                Code = 400,
                IsSuccess = false,
                Message = TotalMessage.ForgotPasswordError,
            };
        }

        [HttpPost("reSendEmail")]
        //Resend Email
        [AllowAnonymous]
        public async Task<ApiResponseModel> ReSendEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action(nameof(ReSendEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                    string userName = user.UserName;
                    // Tạo nội dung HTML cho email
                    string titleContent = "Re-send your password - Opuslink";

                    string emailContent = "Hello " + userName + ",\r\n\r\n" +
                        "Thank you for using Opuslink services. If you have forgotten your password, don't worry! We will help you reset your password easily.\r\n\r\n" +
                        "Please click on the link below to set your new password:" + "\r\n" +
                        passwordResetLink + "\r\n\r\n" +
                        "If you did not request this, please ignore this email.\r\n" +
                        "Thank you for using Opuslink. If you need assistance or have any questions, please feel free to contact us via email support@opuslink.com.\r\n\r\n" +
                        "Regards,\r\nOpuslink Support Team";

                    var message = new MessageEmail(new string[] { user.Email! }, titleContent, emailContent);
                    _emailService.SendEmail(message);
                    return new ApiResponseModel()
                    {
                        IsSuccess = true,
                        Message = "Reset password email sent",
                    };
                }
                return new ApiResponseModel()
                {
                    IsSuccess = false,
                    Message = "Email not found",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("reSendEmail")]
        public async Task<IActionResult> ReSendEmail(string token, string email)
        {
            var model = new ReSendEmailDTO { Token = token, Email = email };
            return Ok(new
            {
                model
            });
        }


        [HttpGet("resetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
                var model = new ResetPassword { Token = token, Email = email  };
            return Ok(new
            {
                model
            });
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<ApiResponseModel> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = string.Join("; ", errors),
                        IsSuccess = false
                    };
                }
                    return new ApiResponseModel()
                {
                    Code = 200,
                    IsSuccess = true,
                    Message = "Reset password sent",
                };
            }
            return new ApiResponseModel()
            {
                Code = 400,
                IsSuccess = false,
                Message = "Email not found",
            };
        }
        

        //Add Token to Verify Email
        private async void SendEmail(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string userName = user.UserName;
            string confirmationLink = UrlConstant.UserClientBaseUrl + "/Account/EmailVerify" + "?token=" + Uri.EscapeDataString(token) + "&email=" + user.Email;

            // Tạo nội dung HTML cho email
            string titleContent = "Xác nhận địa chỉ Email của bạn - Opuslink";

            string emailContent = "Xin chào " + userName + ",\r\n\r\n" +
                "Cảm ơn bạn đã đăng ký tài khoản trên Opuslink, nền tảng tìm việc làm freelancer hàng đầu. " +
                "Để hoàn tất quá trình đăng ký và bắt đầu sử dụng dịch vụ của chúng tôi, bạn cần xác nhận địa chỉ email của mình.\r\n\r\n" +
                "Vui lòng nhấp vào liên kết bên dưới để xác nhận địa chỉ email của bạn :" + "\r\n" +
                confirmationLink + "\r\n\r\n" +
                "Nếu bạn không thực hiện yêu cầu này, bạn có thể bỏ qua email này.\r\n" +
                "Cảm ơn bạn đã sử dụng Opuslink. Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email support@opuslink.com.\r\n\r\n" +
                "Trân trọng,\r\nĐội ngũ hỗ trợ Opuslink";

            // Tạo đối tượng MessageEmail với nội dung HTML
            var message = new MessageEmail(new string[] { user.Email! }, titleContent, emailContent);

            // Gửi email
            _emailService.SendEmail(message);
        }

        [HttpPost("update-role")]
        public async Task<ApiResponseModel> UpdateUserRole(UpdateRoleDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Data = errors,
                        IsSuccess = false,
                        Message = string.Join(";", errors)
                    };
                }
                var result = await _accountService.UpdateUserRole(model);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet("logout")]
        public async Task<ApiResponseModel> Logout()
        {
            await _signInManager.SignOutAsync();
            return new ApiResponseModel()
            {
                Code = 200,
                IsSuccess = true,
                Message = TotalMessage.LogOutSuccess
            };
        }
    }
}
