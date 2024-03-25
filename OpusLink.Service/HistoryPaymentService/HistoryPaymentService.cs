using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.HistoryPaymentService
{
    public interface IHistoryPaymentService
    {
        List<HistoryPayment> getHistoryPayments(); 
        List<HistoryPayment> getHistoryPaymentsByUserId (int id);
    }
    public class HistoryPaymentService: IHistoryPaymentService
    {
        private readonly OpusLinkDBContext _context;
        public HistoryPaymentService(OpusLinkDBContext context)
        {
            _context = context;
        }
        public List<HistoryPayment> getHistoryPayments()
        {
            try
            {
                var hisPayment = _context.HistoryPayments.Include("User").ToList();
                return hisPayment;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<HistoryPayment> getHistoryPaymentsByUserId(int id)
        {
            try
            {
                var hisPayment = _context.HistoryPayments.Include("User").Where(x=> x.UserID == id).ToList();
                return hisPayment;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
