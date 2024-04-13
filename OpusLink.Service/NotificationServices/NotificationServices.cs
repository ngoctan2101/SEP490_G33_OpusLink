using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.NotificationServices
{
    public interface  INotificationServices
    {
        public void AddHNotification(Notification notification);
        public void AddNotification(int UserId,string notificationContent, string link );
        public List<Notification> Get5NotificationNew(int uid);
        public List<Notification> GetAllNotification(int uid);
        public Notification UpdateNotificationReader(int notiId);
        public int CountNotificationNew(int uid);
    }
    public class NotificationServices : INotificationServices
    {
        private readonly OpusLinkDBContext _context = new OpusLinkDBContext();

        public NotificationServices()
        {
        }
        public List<Notification> Get5NotificationNew(int uid)
        {
            try
            {
                var noti = _context.Notifications.Include(x=>x.User).Where(x=>x.UserID == uid && x.NotificationDate.Date ==  DateTime.Today ).OrderByDescending(x=>x.NotificationDate).Take(5).ToList();  
                return noti;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<Notification> GetAllNotification(int uid)
        {
            try
            {
                var noti = _context.Notifications.Include(x => x.User).Where(x => x.UserID == uid).OrderByDescending(x => x.NotificationDate).ToList();
                return noti;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void AddHNotification(Notification notification)
        {
            try
            {

                _context.Notifications.Add(notification);
                _context.SaveChanges();
                
            }
            catch (Exception e)
            {
                throw new Exception("Error adding skill", e);
            }

        }
        public void AddNotification(int userId, string notificationContent, string link)
        {
            Notification n = new Notification()
            {
                NotificationID = 0,
                UserID = userId,
                NotificationContent = notificationContent,
                IsReaded = false,
                Link = link,
                NotificationDate = DateTime.Now
            };
            try
            {

                _context.Notifications.Add(n);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception("Error adding notification", e);
            }

        }
        public Notification UpdateNotificationReader(int notiId)
        {
            try
            {
                Notification noti = _context.Notifications.FirstOrDefault(x => x.NotificationID == notiId);
                noti.IsReaded = true;

                _context.Notifications.Update(noti);
                _context.SaveChanges();
                return noti;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //public int CountNotificationNew()
        public int CountNotificationNew(int uid)
        {
            try
            {
                var noti = _context.Notifications.Include(x => x.User).Where(x => x.UserID == uid && x.NotificationDate.Date == DateTime.Today).OrderByDescending(x => x.NotificationDate).Take(5).ToList();
                var count = noti.Count;
                return count;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
