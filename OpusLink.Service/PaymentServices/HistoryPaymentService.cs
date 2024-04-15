using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.PaymentServices
{
    public interface IHistoryPaymentService
    {
        HistoryPayment GetHistoryPaymentById(int id);
        List<HistoryPayment> GetHistoryPaymentByUserId(int id);
        List<HistoryPayment> GetAllHistoryPayment();
        int AddHistoryPayment(HistoryPayment historyPayment);
        bool CheckExist(List<HistoryPayment> hisList, int targehisListtHislId);
        void UpdateHistoryPayment(HistoryPayment historyPayment);
        void DeleteHistoryPayment(int payId);
		Task<DataIncomePerYear> GetDataIncome(int year);
        Task<List<HistoryPayment>> GetHistory(int month, int year);
    }
    public class HistoryPaymentService : IHistoryPaymentService
    {
        private readonly OpusLinkDBContext _context = new OpusLinkDBContext();

        public HistoryPaymentService()
        {
        }

        public HistoryPayment GetHistoryPaymentById(int id)
        {
            try
            {
                var his = _context.HistoryPayments.Include(x => x.User).FirstOrDefault(x => x.PaymentID == id);

                return his;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<HistoryPayment> GetHistoryPaymentByUserId(int id)
        {
            try
            {
                var his = _context.HistoryPayments.Include(x => x.User).Where(x => x.UserID == id).ToList();

                return his;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<HistoryPayment> GetAllHistoryPayment()
        {
            try
            {
                var his = _context.HistoryPayments.Include(x => x.User).ToList();

                return his;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int AddHistoryPayment(HistoryPayment historyPayment)
        {
            try
            {
                //historyPayment.UserID = 1;
                //historyPayment.Amount = 0;
                //historyPayment.TransactionType = 0;
                //historyPayment.TransactionDate = DateTime.Now;
                //historyPayment.TransactionCode = null;

                _context.HistoryPayments.Add(historyPayment);
                _context.SaveChanges();
                return historyPayment.PaymentID;
            }
            catch (Exception e)
            {
                throw new Exception("Error adding skill", e);
            }

        }
        public void UpdateHistoryPayment(HistoryPayment his)
        {
            try
            {
                his.UserID = 0;
                his.Amount = 0;
                his.TransactionType = 0;
                his.TransactionDate = DateTime.Now;
                his.TransactionCode = null;
                _context.HistoryPayments.Update(his);
                _context.SaveChanges();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void DeleteHistoryPayment(int payId)
        {
            try
            {
                var his = _context.HistoryPayments.Include(x=>x.User).FirstOrDefault(x => x.PaymentID == payId);
                
                _context.HistoryPayments.Remove(his);
                _context.SaveChanges();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CheckExist(List<HistoryPayment> hisList, int targehisListtHislId)
        {
            return hisList.Any(x => x.PaymentID == targehisListtHislId);
        }

		public async Task<DataIncomePerYear> GetDataIncome(int year)
		{
            DataIncomePerYear data = new DataIncomePerYear();
            var hps = await _context.HistoryPayments.Where(h=>h.TransactionDate.Year==year &&  h.TransactionType == 10).ToListAsync();
            foreach(var h in hps)
            {
                if (h.TransactionDate.Month == 1)
                {
                    data.ThangMot += h.Amount;

				}
				else if(h.TransactionDate.Month == 2)
				{
					data.ThangHai += h.Amount;
				}
				else if (h.TransactionDate.Month == 3)
				{
					data.ThangBa += h.Amount;
				}
				else if (h.TransactionDate.Month == 4)
				{
					data.ThangTu += h.Amount;
				}
				else if (h.TransactionDate.Month == 5)
				{
					data.ThangNam += h.Amount;
				}
				else if (h.TransactionDate.Month == 6)
				{
					data.ThangSau += h.Amount;
				}
				else if (h.TransactionDate.Month == 7)
				{
					data.ThangBay += h.Amount;
				}
				else if (h.TransactionDate.Month == 8)
				{
					data.ThangTam += h.Amount;
				}
				else if (h.TransactionDate.Month == 9)
				{
					data.ThangChin += h.Amount;
				}
				else if (h.TransactionDate.Month == 10)
				{
					data.ThangMuoi += h.Amount;
				}
				else if (h.TransactionDate.Month == 11)
				{
					data.ThangMuoiMot += h.Amount;
				}
				else if (h.TransactionDate.Month == 12)
				{
					data.ThangMuoiHai += h.Amount;
				}

			}
            return data;
		}


        public async Task<List<HistoryPayment>> GetHistory(int month, int year)
        {
           return await _context.HistoryPayments.Where(h => h.TransactionDate.Year == year && h.TransactionDate.Month == month && h.TransactionType == 10).ToListAsync();
        }
    }
}
