using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.DTO.AccountDTO.SendEmail;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.AccountServices;
using OpusLink.Service.Admin;
using OpusLink.Service.UserServices;

namespace OpusLink.API.Controllers.Admin
{
    [Authorize(Roles = "Freelancer,Employer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        private IMapper _mapper;
        private ISkillService _skillService;
        private IFreelancerAndSkillService _freelancerAndSkillService;
        private readonly OpusLinkDBContext _context;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IMapper mapper, ISkillService skillService, IFreelancerAndSkillService freelancerAndSkillService, OpusLinkDBContext context, IEmailService emailService)
        {
            _userService = userService;
            _mapper = mapper;
            _skillService = skillService;
            _freelancerAndSkillService = freelancerAndSkillService;
            _emailService = emailService;
            _context = context;
        }
        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            List<OpusLink.Entity.Models.User> users = _userService.GetAllUser();
            var userdto = _mapper.Map<List<UserDTO>>(users);
            if (users != null && users.Count == 0)
            {
                return NotFound("Don't have users");
            }
            string UserImagePath;
            string imageFilePath;
            foreach (UserDTO user in userdto)
            {
                UserImagePath = user.ProfilePicture;
                if (UserImagePath == null || UserImagePath.Length == 0)
                {
                    continue;
                }
                imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", UserImagePath);
                // Check if the file exists
                if (System.IO.File.Exists(imageFilePath))
                {
                    user.UserImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
                }
                
            }
            return Ok(userdto);
        }
        [HttpPost("GetTenUser")]
        public IActionResult GetTenUser([FromBody] Filter filter)
        {
            int numberOfPage;
            List<OpusLink.Entity.Models.User> users = _userService.GetAllUser();
            var userResultAfterSearch = Search(users, filter, out numberOfPage);
            if (users != null && users.Count == 0)
            {
                return Ok("Don't have user");
            }
            List<UserDTO> result = _mapper.Map<List<UserDTO>>(userResultAfterSearch);
            string UserImagePath;
            string imageFilePath;
            foreach (UserDTO user in result)
            {
                UserImagePath = user.ProfilePicture;
                if (UserImagePath == null || UserImagePath.Length == 0)
                {
                    continue;
                }
                imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", UserImagePath);
                // Check if the file exists
                if (System.IO.File.Exists(imageFilePath))
                {
                    user.UserImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
                }

            }
            result.Add(new UserDTO { Id = numberOfPage });
            return Ok(result);
        }
        private object Search(List<OpusLink.Entity.Models.User> users, Filter filter, out int numberOfPage)
        {
            List<OpusLink.Entity.Models.User> result = new List<OpusLink.Entity.Models.User>();
            result = users.ToList();
            //search string
            if (filter.SearchStr.Length > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].UserName.ToLower().Contains(filter.SearchStr.ToLower()))
                    {
                        continue;
                    }
                    else
                    {
                        result.RemoveAt(i);
                    }
                }
            }
            //loc theo page
            numberOfPage = result.Count / 10;
            if (result.Count % 10 > 0)
            {
                numberOfPage++;
            }
            return result.Skip((filter.PageNumber - 1) * 10).Take(10).ToList();
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {

            var user = _userService.GetUserById(id);
            var userdto = _mapper.Map<UserDTO>(user);
            if (user == null)
            {
                return NotFound("Don't have user");
            }
            foreach (FreelancerAndSkill fas in user.FreelancerAndSkills)
            {
                userdto.Skills.Add(_mapper.Map<SkillDTO>(fas.Skill));
            }
            string UserImagePath = userdto.ProfilePicture;
            if (UserImagePath == null || UserImagePath.Length == 0)
            {
                return Ok(userdto);
            }
            string imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", UserImagePath);
            // Check if the file exists
            if (System.IO.File.Exists(imageFilePath))
            {
                userdto.UserImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
            }
            return Ok(userdto);
        }
        [HttpGet("GetUserByName/{txt}")]
        public IActionResult GetUserByName(string txt)
        {
            txt.Trim();
            var user = _userService.GetUserByName(txt);
            var userdto = _mapper.Map<List<UserDTO>>(user);
            if (user != null && user.Count == 0)
            {
                return NotFound("Don't have users");
            }
            string UserImagePath;
            string imageFilePath;
            foreach (UserDTO user1 in userdto)
            {
                UserImagePath = user1.ProfilePicture;
                if (UserImagePath == null || UserImagePath.Length == 0)
                {
                    continue;
                }
                imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", UserImagePath);
                // Check if the file exists
                if (System.IO.File.Exists(imageFilePath))
                {
                    user1.UserImageBytes = System.IO.File.ReadAllBytes(imageFilePath);
                }

            }
            return Ok(userdto);
        }
        [HttpGet("GetFileCVById/{userId}")]
        public IActionResult GetFileCVById(int userId)
        {
            string filePath = _userService.GetUserById(userId).CVFilePath;
            string cvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", filePath);

            // Check if the file exists
            if (!System.IO.File.Exists(cvFilePath))
            {
                return NotFound();
            }

            // Determine the content type based on file extension
            var contentType = "application/octet-stream"; // Default content type
            if (filePath.EndsWith(".pdf"))
            {
                contentType = "application/pdf";
            }
            else if (filePath.EndsWith(".docx"))
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }

            // Stream the file to the client
            var fileStream = new FileStream(cvFilePath, FileMode.Open);
            return File(fileStream, contentType, filePath);
        }
        [HttpPut("PutUserAdmin")]
        public async Task<IActionResult> PutUserAdmin([FromBody] PutUserRequest putUserRequest)
        {
            //update skill
            List<FreelancerAndSkill> fass = await _freelancerAndSkillService.getAllFASOfUser(putUserRequest.Id);
                //find list fas to delete
            List<FreelancerAndSkill> fasd = FindFAS2Delete(fass, putUserRequest.SkillIDs);
                //find list fas to add
            List<FreelancerAndSkill> fasa = FindFAS2Add(fass, putUserRequest.SkillIDs, putUserRequest.Id);
            await _freelancerAndSkillService.DeleteRangeAsync(fasd);
            await _freelancerAndSkillService.AddRangeAsync(fasa);
            //cv and image
            OpusLink.Entity.Models.User thisUser= _userService.GetUserById(putUserRequest.Id);
            thisUser.Introduction=putUserRequest.Introduction;
            string cvFilePath=""; 
            string imageFilePath="";
            if (putUserRequest.UserCVBytes != null)
            {
                if (String.IsNullOrEmpty(thisUser.CVFilePath))
                {
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", "r"+thisUser.Id+putUserRequest.cvExtension)
                        , putUserRequest.UserCVBytes);
                    //them ten file
                    thisUser.CVFilePath = "r" + thisUser.Id + putUserRequest.cvExtension;
                }
                else
                {
                    //xoa file cu
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", thisUser.CVFilePath));
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", "r" + thisUser.Id + putUserRequest.cvExtension)
                        , putUserRequest.UserCVBytes);
                    //them ten file
                    thisUser.CVFilePath = "r" + thisUser.Id + putUserRequest.cvExtension;

                }
            }
            if (putUserRequest.UserImageBytes != null)
            {
                if (String.IsNullOrEmpty(thisUser.ProfilePicture))
                {
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", "i" + thisUser.Id + putUserRequest.imageExtension)
                        , putUserRequest.UserImageBytes);
                    //them ten file
                    thisUser.ProfilePicture = "i" + thisUser.Id + putUserRequest.imageExtension;
                }
                else
                {
                    //xoa file cu
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", thisUser.ProfilePicture));
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", "i" + thisUser.Id + putUserRequest.imageExtension)
                        , putUserRequest.UserImageBytes);
                    //them ten file
                    thisUser.ProfilePicture = "i" + thisUser.Id + putUserRequest.imageExtension;

                }
            }
            _userService.UpdateOnlyUserIntroductionFileCVAndImage(thisUser);
            return Ok();
        }

        [HttpPut("PutUserUser")]
        public async Task<IActionResult> PutUserUser([FromBody] PutUserRequest putUserRequest)
        {
            //update skill
            List<FreelancerAndSkill> fass = await _freelancerAndSkillService.getAllFASOfUser(putUserRequest.Id);
            //find list fas to delete
            List<FreelancerAndSkill> fasd = FindFAS2Delete(fass, putUserRequest.SkillIDs);
            //find list fas to add
            List<FreelancerAndSkill> fasa = FindFAS2Add(fass, putUserRequest.SkillIDs, putUserRequest.Id);
            await _freelancerAndSkillService.DeleteRangeAsync(fasd);
            await _freelancerAndSkillService.AddRangeAsync(fasa);
            //cv and image
            OpusLink.Entity.Models.User thisUser = _userService.GetUserById(putUserRequest.Id);
            thisUser.Introduction = putUserRequest.Introduction;
            thisUser.Email = putUserRequest.Email;
            thisUser.Address = putUserRequest.Address;
            thisUser.FullNameOnIDCard = putUserRequest.FullNameOnIDCard;
            thisUser.PhoneNumber = putUserRequest.PhoneNumber;
            thisUser.Dob = putUserRequest.Dob;
            thisUser.BankName = putUserRequest.BankName;
            thisUser.BankAccountInfor = putUserRequest.BankAccountInfor;
            string cvFilePath = "";
            string imageFilePath = "";
            if (putUserRequest.UserCVBytes != null)
            {
                if (String.IsNullOrEmpty(thisUser.CVFilePath))
                {
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", "r" + thisUser.Id + putUserRequest.cvExtension)
                        , putUserRequest.UserCVBytes);
                    //them ten file
                    thisUser.CVFilePath = "r" + thisUser.Id + putUserRequest.cvExtension;
                }
                else
                {
                    //xoa file cu
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", thisUser.CVFilePath));
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\resume", "r" + thisUser.Id + putUserRequest.cvExtension)
                        , putUserRequest.UserCVBytes);
                    //them ten file
                    thisUser.CVFilePath = "r" + thisUser.Id + putUserRequest.cvExtension;

                }
            }
            if (putUserRequest.UserImageBytes != null)
            {
                if (String.IsNullOrEmpty(thisUser.ProfilePicture))
                {
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", "i" + thisUser.Id + putUserRequest.imageExtension)
                        , putUserRequest.UserImageBytes);
                    //them ten file
                    thisUser.ProfilePicture = "i" + thisUser.Id + putUserRequest.imageExtension;
                }
                else
                {
                    //xoa file cu
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", thisUser.ProfilePicture));
                    //tao file moi
                    System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "FilesUserUpload\\profileImage", "i" + thisUser.Id + putUserRequest.imageExtension)
                        , putUserRequest.UserImageBytes);
                    //them ten file
                    thisUser.ProfilePicture = "i" + thisUser.Id + putUserRequest.imageExtension;

                }
            }
            _userService.UpdateUser2(thisUser);
            return Ok();
        }
        [HttpPut("UpdateAmount/{amount}/{userId}")]
        public async Task<IActionResult> UpdateAmount(double amount, int userId)
        {
            User us = _userService.GetUserById(userId);
            if (us != null)
            {
                if (amount >= 0 )
                {
                    _userService.UpdateAmountMoney(amount, userId);
                    return Ok("Loaded money successfull");

                }
                else
                {
                    NotFound("Money not accept");
                }

            }


            return Ok("Not find User");
        }

        [HttpPut("WithdrawMoney/{amount}/{userId}")]
        public async Task<IActionResult> WithdrawMoney(double amount, int userId)
        {
            User us = _userService.GetUserById(userId);
            if (us != null)
            {
                if(amount >= 0 && us.AmountMoney >= Convert.ToDecimal(amount))
                {
                    _userService.WithdrawMoney(amount, userId);
                     return Ok("Withdraw money successfull");
                }
                else
                {
                    NotFound("Insufficient account");
                }     
            }
            return Ok("Not find User");
        }

        [HttpPatch("BanUser1")]
        public async Task<IActionResult> UpdateBanUser1(string banReason, DateTime endBanDate, int userId)
        {
            bool isCheck = _userService.UpdateBanUser1(banReason, endBanDate, userId);
            if(isCheck == false)
            {
                return BadRequest("Cannot update because this user account has job");
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            string titleContent = "Tài khoản bị khóa - Opuslink";

            string emailContent = "Xin chào " + user.UserName + ",\r\n\r\n" +
                "Cảm ơn bạn đã sử dụng dịch vụ của Opuslink, nền tảng tìm việc làm freelancer hàng đầu.\r\n" +
                "Chúng tôi gửi thông báo rằng tài khoản của bạn đã bị khóa từ giờ cho đến " + endBanDate.ToString("dd/MM/yyyy") + "\r\nLý do khóa tài khoản: " +
                banReason + ".\r\n\r\n" +
                "Xin lưu ý rằng bạn sẽ không thể truy cập vào tài khoản của mình cho đến " + endBanDate.ToString("dd/MM/yyyy") + "\r\n" +
                "Để biết thêm thông tin chi tiết và khôi phục tài khoản, vui lòng liên hệ với bộ phận hỗ trợ của chúng tôi.\r\n" +
                "Cảm ơn bạn đã hiểu và hợp tác trong quá trình này. Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi qua email support@opuslink.com.\r\n\r\n" +
                "Trân trọng,\r\nĐội ngũ hỗ trợ Opuslink";

            var message = new MessageEmail(new string[] { user.Email! }, titleContent, emailContent);
            _emailService.SendEmail(message);
            return Ok("Update successfull");
        }

        [HttpPatch("BanUser")]
        public async Task<IActionResult> UpdateBanUser(string banReason, DateTime endBanDate, int userId)
        {
            _userService.UpdateBanUser(banReason, endBanDate, userId);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            string titleContent = "Tài khoản bị khóa - Opuslink";

            string emailContent = "Xin chào " + user.UserName + ",\r\n\r\n" +
                "Cảm ơn bạn đã sử dụng dịch vụ của Opuslink, nền tảng tìm việc làm freelancer hàng đầu.\r\n" +
                "Chúng tôi gửi thông báo rằng tài khoản của bạn đã bị khóa từ giờ cho đến " + endBanDate.ToString("dd/MM/yyyy") + "\r\nLý do khóa tài khoản: " +
                banReason + ".\r\n\r\n" +
                "Xin lưu ý rằng bạn sẽ không thể truy cập vào tài khoản của mình cho đến " + endBanDate.ToString("dd/MM/yyyy") + "\r\n" +
                "Để biết thêm thông tin chi tiết và khôi phục tài khoản, vui lòng liên hệ với bộ phận hỗ trợ của chúng tôi.\r\n" +
                "Cảm ơn bạn đã hiểu và hợp tác trong quá trình này. Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi qua email support@opuslink.com.\r\n\r\n" +
                "Trân trọng,\r\nĐội ngũ hỗ trợ Opuslink";

            var message = new MessageEmail(new string[] { user.Email! }, titleContent, emailContent);
            _emailService.SendEmail(message);
            return Ok("Update successfull");
        }

        [HttpPatch("UnBanUser")]
        public async Task<IActionResult> UpdateUnBanUser(int userId)
        {
            _userService.UpdateUnBanUser(userId);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            string titleContent = "Tài khoản đã được mở khóa - Opuslink";

            string emailContent = "Xin chào " + user.UserName + ",\r\n\r\n" +
                "Cảm ơn bạn đã sử dụng dịch vụ của Opuslink, nền tảng tìm việc làm freelancer hàng đầu. " +
                "Chúng tôi muốn thông báo rằng tài khoản của bạn đã được mở khóa trở lại.\r\n" +
                "Bạn đã có thể truy cập vào tài khoản của mình bắt đầu từ bây giờ.\r\n\r\n" +
                "Nếu bạn có bất kỳ câu hỏi nào hoặc cần hỗ trợ thêm, vui lòng liên hệ với bộ phận hỗ trợ của chúng tôi qua email support@opuslink.com.\r\n" +
                "Cảm ơn bạn đã kiên nhẫn và hợp tác trong quá trình này.\r\n\r\n" +
                "Trân trọng,\r\nĐội ngũ hỗ trợ Opuslink";

            var message = new MessageEmail(new string[] { user.Email! }, titleContent, emailContent);
            _emailService.SendEmail(message);
            return Ok("Update successfull");
        }
        private List<FreelancerAndSkill> FindFAS2Add(List<FreelancerAndSkill> fasa, List<int> skillIDs, int userId)
        {
            List<FreelancerAndSkill> fasResult = new List<FreelancerAndSkill>();
            foreach (int fas in skillIDs)
            {
                if (!(fasa.Select(a => a.SkillID).ToList()).Contains(fas))
                {
                    fasResult.Add(new FreelancerAndSkill { FreelancerAndSkillID = 0, FreelancerID = userId, SkillID = fas });
                }
            }
            return fasResult;
        }

        private List<FreelancerAndSkill> FindFAS2Delete(List<FreelancerAndSkill> fasd, List<int> skillIDs)
        {
            List<FreelancerAndSkill> fasResult = new List<FreelancerAndSkill>();
            foreach (FreelancerAndSkill fas in fasd)
            {
                if (!skillIDs.Contains(fas.SkillID))
                {
                    fasResult.Add(fas);
                }
            }
            return fasResult;
        }

        [HttpPut("UpdateBankAccountUser")]
        public async Task<IActionResult> UpdateBankAccountUser([FromBody] BankAccDTO usedto)
        {

            _userService.UpdateBankAccUser(usedto);

            return Ok("Update thanh cong");
        }

    }


}
