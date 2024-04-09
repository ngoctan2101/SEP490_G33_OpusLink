using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Service.AccountServices;

namespace OpusLink.API.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AdminAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("loginAdmin")]
        public async Task<ApiResponseModel> LoginAdmin([FromBody] LoginDTO model)
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
                var result = await _accountService.LoginAdmin(model);
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
