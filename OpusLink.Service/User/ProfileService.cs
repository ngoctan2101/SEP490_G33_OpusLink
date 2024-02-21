using OpusLink.Entity;
using OpusLink.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.User
{
    public interface IProfileService
    {

    }
    public class ProfileService : IProfileService
    {
        private readonly OpusLinkDBContext _context = new OpusLinkDBContext();
        
        public ProfileService()
        {
            
        }

    }
}
