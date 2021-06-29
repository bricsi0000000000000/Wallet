namespace Wallet.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int MaxMoney { get; set; }
        public int SpentMoney { get; set; }
    }
}
