using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusLink.Entity;
using OpusLink.Entity.Models;
namespace OpusLink.Service.Admin
{
    public interface IUserService
    {
        List<OpusLink.Entity.Models.User> GetAllUser();
        OpusLink.Entity.Models.User? GetUserById(int id);
    }

    public class UserService : IUserService
    {
        private readonly OpusLinkDBContext _context;
        public UserService(OpusLinkDBContext context)
        {
            context = _context;
        }

        List<Entity.Models.User> IUserService.GetAllUser()
        {
            throw new NotImplementedException();
        }

        Entity.Models.User? IUserService.GetUserById(int id)
        {
            throw new NotImplementedException();
        }
    }

    
}
