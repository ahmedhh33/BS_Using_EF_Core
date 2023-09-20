using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_EF_Core
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public int SourceAccountNumber { get; set; }
        public int TargetAccountNumber { get; set; }


        [ForeignKey("account")]
        public int AccountNumber { get; set; }
        public Account account { get; set; }

    }
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
