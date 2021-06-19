using SQLite;

namespace Wallet.Models
{
    public class Finance
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Money { get; set; }
        public int CategoryId { get; set; }
        public bool IsExpense { get; set; }
    }
}
