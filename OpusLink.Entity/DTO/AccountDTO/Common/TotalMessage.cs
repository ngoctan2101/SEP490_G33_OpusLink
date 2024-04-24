using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        public static string RegisterAccountNameExists = "Email này đã được đăng kí trước đó. Vui lòng đăng nhập hoặc đăng kí với email khác";
        public static string RegisterAccountEmailExists = "Tên tài khoản này đã được đăng kí trước đó. Vui lòng đăng nhập hoặc đăng kí với tên tài khoản khác";

        //Khi đăng kí nhưng có lỗi khác
        public static string RegisterError = "Lỗi khi tạo tài khoản, vui lòng thử lại";
        public static string RegisterNotEnoughAge = "Bạn chưa đủ độ tuổi để đăng kí tài khoản";


        public static string LoginError = "Tên đăng nhập hoặc Mật khẩu không đúng!";
        public static string LoginFailCuzBanUser = "Tài khoản của bạn đang bị khóa cho đến ";
        public static string LogingErrorRoleAdmin = "Tài khoản Admin không thể đăng nhập với trang này";
        public static string LoginErrorRoleUser = "Tài khoản của bạn không có quyền đăng nhập trang này";
        public static string LoginErrorEmailConfirm = "Bạn chưa xác thực Email cho tài khoản này.";

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
