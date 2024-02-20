using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.HaiDTO;
using OpusLink.Service.Users;

namespace OpusLink.API.Controllers.HaiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _service;

        public AccountController(IAccountService userService)
        {
            _service = userService;
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
                var result = await _service.Login(model);
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
                var result = await _service.Register(model);
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
    }
}
