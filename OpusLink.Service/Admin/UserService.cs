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
        void UpdateUser2(Entity.Models.User a);
        void UpdateAmountMoney(double money,int userId);
        public void WithdrawMoney(double money, int userId);
        void UpdateBanUser(string banReason, DateTime endBanDate, int userId);
        void UpdateUnBanUser(int userId);
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
                var user = _context.Users.Include(u => u.ReportUsersAsATargeter).ToList();
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

        public void UpdateUser2(Entity.Models.User a)
        {
            Entity.Models.User user = _context.Users.Where(u => u.Id == a.Id).FirstOrDefault();
        
            user.Email= a.Email;
            user.ProfilePicture= a.ProfilePicture;
            user.Address= a.Address;
            user.FullNameOnIDCard = a.FullNameOnIDCard;
            user.Dob = a.Dob;
            user.PhoneNumber = a.PhoneNumber;
            user.Introduction = a.Introduction;
            user.CVFilePath = a.CVFilePath;
            user.BankName = a.BankName;
            user.BankAccountInfor = a.BankAccountInfor;
           
           

      
            _context.SaveChanges();
        }

        public void UpdateAmountMoney(double money, int userId)
        {
            
                Entity.Models.User user = _context.Users.FirstOrDefault(u => u.Id == userId);
                //if (user != null)
                //{
                    
                        //if (money >= 0){
                            user.AmountMoney += Convert.ToDecimal(money);
                        //}
                        //else if (money <= 0 && Convert.ToDecimal(money) >= user.AmountMoney)
                        //{
                        //    return;
                        //    // Không thực hiện gì cả vì số tiền trừ không được lớn hơn số tiền hiện có của người dùng
                        //}
                        //else
                        //{
                        //    user.AmountMoney -= Convert.ToDecimal(Math.Abs(money));
                        //}

                        _context.Users.Update(user);
                        _context.SaveChanges();
                    
                    
                //}
                //else
                //{
                //    throw new Exception("User not found");
                //}




        }

        public void WithdrawMoney(double money, int userId)
        {

            Entity.Models.User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            //if (user != null)
            //{

            //    if (money >= 0 && Convert.ToDecimal(money) <= user.AmountMoney)
            //    {
                    user.AmountMoney -= Convert.ToDecimal(money);
                //}
                //else
                //{
                //    throw new Exception();
                //}

                _context.Users.Update(user);
                _context.SaveChanges();


            //}
            //else
            //{
            //    throw new Exception("Your account is not enough");
            //}




        }

        //public void WithdrawMoney(double money, int userId)
        //{

        //    Entity.Models.User user = _context.Users.FirstOrDefault(u => u.Id == userId);
        //    //if (user != null)
        //    //{

        //    //    if (money >= 0 && Convert.ToDecimal(money) <= user.AmountMoney)
        //    //    {
        //            user.AmountMoney -= Convert.ToDecimal(money);
        //        //}
        //        //else
        //        //{
        //        //    throw new Exception();
        //        //}

        //        _context.Users.Update(user);
        //        _context.SaveChanges();


        //    //}
        //    //else
        //    //{
        //    //    throw new Exception("Your account is not enough");
        //    //}




        //}

        public void UpdateBanUser(string banReason, DateTime endBanDate, int userId)
        {
            Entity.Models.User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.BanReason = banReason;
                user.EndBanDate = endBanDate;
                user.Status = 0;

                _context.Update(user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public void UpdateUnBanUser(int userId)
        {
            Entity.Models.User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Status = 1;

                _context.Update(user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("User not found");
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

    

