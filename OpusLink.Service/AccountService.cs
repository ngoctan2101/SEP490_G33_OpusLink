using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service
{
    public interface IAccountService
    {
        //List<Account>? GetAccountsByKeyword(string keyword);
        //List<Account>? GetAllAccount();
        //Account? GetAccountById(int id);
        //Account? GetAccountByEmailAndPassword(string email, string password);
        //Account? GetAccountByEmail(string email);
        //CreateAccountResponse AddAccount(CreateAccountRequest account);
        //UpdateAccountResponse UpdateAccount(UpdateAccountRequest account);
        //bool UpdateDeleteStatusAccount(int id);
        //List<Account> GetAccountsByRoleId(int roleId);
    }
    internal class AccountService : IAccountService
    {
        //private readonly FstoreContext _context;
        //private readonly IEncryptionService _encryptionService;

        //public AccountService(FstoreContext context, IEncryptionService encryptionService)
        //{
        //    _context = context;
        //    _encryptionService = encryptionService;
        //}


        //public List<Account> GetAccountsByKeyword(string keyword)
        //{
        //    try
        //    {
        //        var accounts = _context.Accounts.Where(x => x.FullName.ToLower().Contains(keyword.ToLower())).ToList();
        //        return accounts;

        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}
    }
}
