using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using OpusLink.Service.JobServices;
using OpusLink.Service.User;

namespace OpusLink.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        private IMapper _mapper;
        private ISkillService _skillService;
        private IFreelancerAndSkillService _freelancerAndSkillService;
        public UserController(IUserService userService, IMapper mapper, ISkillService skillService, IFreelancerAndSkillService freelancerAndSkillService)
        {
            _userService = userService;
            _mapper = mapper;
            _skillService = skillService;
            _freelancerAndSkillService = freelancerAndSkillService;
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
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {

            var user = _userService.GetUserById(id);
            var userdto = _mapper.Map<UserDTO>(user);
            foreach (FreelancerAndSkill fas in user.FreelancerAndSkills)
            {
                userdto.Skills.Add(_mapper.Map<SkillDTO>(fas.Skill));
            }
            if (user == null)
            {
                return NotFound("Don't have user");
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
        
    }


}
