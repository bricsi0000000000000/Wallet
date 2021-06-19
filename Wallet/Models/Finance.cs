using System;

namespace Wallet.Models
{
    public class Finance
    {
        public int Id { get; set; }
        public int Money { get; set; }
        public int CategoryId { get; set; }
        public bool IsExpense { get; set; }
        public DateTime Date { get; set; }
    }
}
