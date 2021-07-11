using System.Collections.Generic;

namespace Wallet.Models
{
    public class FinanceCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }

        public List<int> SubCategoryIds { get; set; }
    }
}
