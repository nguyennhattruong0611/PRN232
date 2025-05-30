using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.Models;

namespace Repositories.IRepository
{
    public interface IAccountRepository
    {
        AccountMember GetAccountById(string accountId);
    }
}
