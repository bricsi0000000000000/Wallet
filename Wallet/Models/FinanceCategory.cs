using SQLite;

namespace Wallet.Models
{
    public class FinanceCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
    }
}
