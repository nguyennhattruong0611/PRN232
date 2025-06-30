using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class SystemAccountDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountRole { get; set; }
        public string Token { get; set; } 

    }
}
