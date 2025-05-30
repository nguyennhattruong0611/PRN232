using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DAO;
using DataAccessObjects.Models;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public AccountMember GetAccountById(string accountID)
            => AccountDAO.GetAccountById(accountID);
    }
}
