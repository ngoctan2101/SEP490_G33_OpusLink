using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.Models;
using System.Security.Principal;

namespace OpusLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        [HttpPost]
        public IActionResult PostAccount(AccountAddDTO accountdto)
        {
            //User account = new User
            //{
            //    FullName = accountdto.FullName,
            //    Email = accountdto.Email,
            //    Address = accountdto.Address,
            //    Dob = accountdto.Dob,
            //    Gender = accountdto.Gender,
            //    Phone = accountdto.Phone,
            //    Password = accountdto.Password,
            //    Active = accountdto.Active,
            //    Role = accountdto.Role

            //};
            //AccountDAO.Instance.AddAccount(account);
            return Ok();
        }
    }
}
