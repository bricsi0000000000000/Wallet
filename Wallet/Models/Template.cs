namespace Wallet.Models
{
    public enum FinanceType
    {
        Expense = 0,
        Income = 1
    }

    public class Template
    {
        public int Id { get; set; }
        public int Money { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public FinanceType Type { get; set; }

        public override string ToString()
        {
            return $"{Description} ({Money.FormatToMoney()})";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Template);
        }

        public bool Equals(Template other)
        {
            return other != null &&
                   Money == other.Money &&
                   Description.Trim() == other.Description.Trim() &&
                   CategoryId == other.CategoryId &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
