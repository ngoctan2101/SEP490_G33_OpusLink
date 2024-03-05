using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Service.AccountServices;
using OpusLink.Entity.DTO.AccountDTO.SendEmail;
using OpusLink.Entity.Models;
using Microsoft.AspNetCore.Identity;
using OpusLink.Shared.Enums;
using System.Text;
using System.Runtime.InteropServices;

namespace OpusLink.API.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, IAccountService userService, IEmailService emailService)
        {
            _userManager = userManager;
            _accountService = userService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<ApiResponseModel> Login([FromBody] LoginDTO model)
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
                var result = await _accountService.Login(model);
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

        [HttpPost("register")]
        public async Task<ApiResponseModel> Register([FromBody] RegisterDTO model)
        {
            try
            {
                ApiResponseModel result = new ApiResponseModel()
                {
                    Code = 200,
                    IsSuccess = true,
                    Message = "User created success",
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
                        Message = "User has been already existed!",
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
                };

                var resultCreateUser = await _userManager.CreateAsync(user, model.Password);

                //Role mặc định là Freelancer and Employer
                var resultRoleFreelancer = await _userManager.AddToRoleAsync(user, Roles.Freelancer.ToString());
                var resultRoleEmployer = await _userManager.AddToRoleAsync(user, Roles.Employer.ToString());

                //Role cho Admin
                /*var resultRoleEmployer = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());*/
                if (!resultCreateUser.Succeeded)
                {
                    return new ApiResponseModel()
                    {
                        Code = 400,
                        Message = "Error when create user",
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

        //Add Token to Verify Email
        private async void SendEmail(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var userName = user.UserName;
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

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
    }
}
