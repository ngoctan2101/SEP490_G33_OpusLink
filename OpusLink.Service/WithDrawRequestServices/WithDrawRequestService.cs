using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO.WithdrawRequestDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.WithDrawRequestServices
{
    public interface IWithDrawRequestService
    {
        public List<WithdrawRequest> GetAllWithdrawRequestByStatus(int status);
        public WithdrawRequest GetAllWithdrawRequestById(int wId);
        public void AddWithdrawRequest(WithdrawRequest wdr);
        public bool UpdateHWithdrawRequestByStatusToSuccessfull(int WId);
        public bool UpdateHWithdrawRequestByStatusToFail(int WId, string txt);
        public void UpdateHisIdWithdrawRequest(int wid, int his);


    }
    public class WithDrawRequestService : IWithDrawRequestService
    {
        public readonly OpusLinkDBContext _context = new OpusLinkDBContext();

        public IHistoryPaymentService _historyPayment = new HistoryPaymentService();

        public List<WithdrawRequest> GetAllWithdrawRequestByStatus(int status)
        {
            
                var withdraw =  _context.WithdrawRequests.Where(x=>x.Status == status).OrderByDescending(x => x.WithdrawRequestID).Include("User").ToList();
                return withdraw;

            
           
        }

        public WithdrawRequest GetAllWithdrawRequestById(int wId)
        {
            try
            {
                var withdraw = _context.WithdrawRequests.Include(x => x.User).FirstOrDefault(x => x.WithdrawRequestID == wId);

                return withdraw;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddWithdrawRequest(WithdrawRequest wdr)
        {
            try
            {
                
                _context.WithdrawRequests.Add(wdr);
                _context.SaveChanges();
              
            }
            catch (Exception e)
            {
                throw new Exception("Error adding WithdrawRequest", e);
            }
        }
        public void UpdateHisIdWithdrawRequest(int wid , int his)
        {
            try
            {
                var history = _historyPayment.GetHistoryPaymentById(his); 
                var reponWith = GetAllWithdrawRequestById(wid);
                if (reponWith != null && history != null)
                {
                    reponWith.HistoryPaymentID = his;
                }
                else throw new Exception();
                
                
                _context.WithdrawRequests.Update(reponWith);
                _context.SaveChanges();
                
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }

        public bool UpdateHWithdrawRequestByStatusToSuccessfull(int WId)
        {
            try
            {
                var withdraw = GetAllWithdrawRequestById(WId);
                if (withdraw == null)
                {
                    return false;
                }
                if (withdraw.Status != 1) { return false; }
                if (withdraw.Status == 1)
                {
                    withdraw.Status = 2;
                }
                _context.WithdrawRequests.Update(withdraw);
                _context.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateHWithdrawRequestByStatusToFail(int WId, string txt)
        {
            try
            {
                var withdraw = GetAllWithdrawRequestById(WId);
                if (withdraw == null)
                {
                    return false;
                }
                if(withdraw.Status != 1) { return false; }
                if (withdraw.Status == 1)
                {
                    withdraw.Status = 3;
                    withdraw.Reason = txt;

                }
                _context.WithdrawRequests.Update(withdraw);
                _context.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
