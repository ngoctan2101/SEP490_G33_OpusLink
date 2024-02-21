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
    }



    public class UserService : IUserService
    {
        private readonly OpusLinkDBContext _context = new OpusLinkDBContext();
        private readonly ISkillService _skillService;
        public UserService(ISkillService skillService)
        {
            _skillService = skillService;
        }
        public List<OpusLink.Entity.Models.User> GetUserByName(string txt)
        {
            try
            {
                txt.Trim();
                if (txt != null)
                {
                    var user = _context.Users.Where(x=>x.UserName.ToLower().Contains(txt.ToLower())).ToList();

                    return user;

                }
                else throw new Exception();
                

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Entity.Models.User? GetUserById(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == id);
                
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

    

