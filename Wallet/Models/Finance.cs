using System;

namespace Wallet.Models
{
    public enum FinanceType
    {
         Expense = 0,
         Income = 1,
         Deposit = 2
    }

    public class Finance
    {
        public int Id { get; set; }
        public int Money { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public FinanceType Type { get; set; }
        public DateTime Date { get; set; }
        public bool IsAutomatized { get; set; }
    }
}
