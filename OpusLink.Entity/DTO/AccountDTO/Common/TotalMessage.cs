using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO.Common
{
    public class TotalMessage
    {
        //---Register---
        //Khi đăng kí tài khoản thành công
        public static string RegisterSuccess = "Đăng kí tài khoản thành công";

        //Khi đăng kí nhưng tài khoản đã tồn tại
        public static string RegisterAccountExists = "Tài khoản này đã tồn tại";

        //Khi đăng kí nhưng có lỗi khác
        public static string RegisterError = "Lỗi khi tạo tài khoản, vui lòng thử lại";


        public static string LoginError = "Tên đăng nhập hoặc Mật khẩu không đúng!";

        //---ForgotPassword---
        //Khi quên mật khẩu thành công
        public static string ForgotPasswordSuccess = "Yêu cầu đặt lại mật khẩu của bạn đã thành công. Vui lòng kiểm tra Email";

        //Khi không tìm thấy email nhập vào quên mật khẩu 
        public static string ForgotPasswordError = "Email không đúng";

        //---LogOut---
        //Khi quên mật khẩu thành công
        public static string LogOutSuccess = "Đăng xuất thành công";


    }
}
