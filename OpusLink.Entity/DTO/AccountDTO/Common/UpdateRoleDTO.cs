using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO.Common
{
    public class UpdateRoleDTO
    {
        public string UserName { get; set; } // Tên người dùng cần cập nhật vai trò
        public string CurrentRole { get; set; } // Vai trò hiện tại của người dùng
    }
}
