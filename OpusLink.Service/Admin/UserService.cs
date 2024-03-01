using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
namespace OpusLink.Service.Admin
{
    public interface IUserService
    {
        List<OpusLink.Entity.Models.User> GetAllUser();
        OpusLink.Entity.Models.User GetUserById(int id);
        List<OpusLink.Entity.Models.User> GetUserByName(string txt);
        void UpdateOnlyUserIntroductionFileCVAndImage(Entity.Models.User a);
    }



    public class UserService : IUserService
    {
        private readonly OpusLinkDBContext _context = new OpusLinkDBContext();
        public UserService()
        {
        }

        public Entity.Models.User? GetUserById(int id)
        {
            try
            {
                var user = _context.Users.Where(u=>u.Id==id).Include("FreelancerAndSkills").Include("FreelancerAndSkills.Skill").FirstOrDefault();
                
                return user;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        

        List<Entity.Models.User> IUserService.GetAllUser()
        {
            try
            {
                var user = _context.Users.ToList();
                return user;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdateOnlyUserIntroductionFileCVAndImage(Entity.Models.User a)
        {
            Entity.Models.User user=_context.Users.Where(u=>u.Id==a.Id).FirstOrDefault();
            user.Introduction= a.Introduction;
            user.CVFilePath= a.CVFilePath;
            user.ProfilePicture= a.ProfilePicture;
            _context.SaveChanges();
        }
    }

        //Entity.Models.User? IUserService.GetUserById(int id)
        //{
        //    try
        //    {
        //        var user = _context.Users.FirstOrDefault(x => x.Id == id);
        //        return user;

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}
    }

    

