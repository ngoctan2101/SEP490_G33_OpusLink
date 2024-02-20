using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpusLink.Entity.DTO.HaiDTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.Users
{
    public interface IDataUpdate
    {
        //Check xem có Role chưa? Chưa có thì mới add mặc định Role
        Task UpdateData();
    }

    public class RoleDataUpdate : IDataUpdate
    {
        private readonly IServiceProvider _app;
        public RoleDataUpdate(IServiceProvider app)
        {
            _app = app;
        }

        public async Task UpdateData()
        {
            var scope = _app.CreateScope();

            //IdentityRole : Định nghĩa ra Role nào
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            //Lấy ra list roles từ cái UserRoles (đại khái là có bao nhiêu roles sẽ cho vào hết list)
            var roles = UserRoles.Roles;
            foreach (var role in roles)
            {
                //Nếu role chưa tồn tại, thì mình sẽ create cái role đấy
                if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    await roleManager.CreateAsync(new Role(role));
                }
            }
        }
    }
}
