using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.Models;

namespace DataAccessObjects.DAO
{
    public class AccountDAO
    {
        public static AccountMember GetAccountById(string accountID)
        {
            using var db = new  Lab1Prn232Context();
            return db.AccountMembers.FirstOrDefault(c => c.MemberId.Equals(accountID));
        }
    }
}
