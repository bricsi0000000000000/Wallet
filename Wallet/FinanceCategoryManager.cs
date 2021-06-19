using System.Collections.Generic;
using Wallet.Models;

namespace Wallet
{
    public static class FinanceCategoryManager
    {
        public static List<FinanceCategory> Categories { get; private set; } = new List<FinanceCategory>();

        public static void Add(FinanceCategory financeCategory)
        {
            Categories.Add(financeCategory);
        }

        public static FinanceCategory Get(int id)
        {
            return Categories.Find(x => x.Id == id);
        }
    }
}
