using System.Collections.Generic;
using System.Linq;
using Wallet.Models;

namespace Wallet
{
    public static class FinanceCategoryManager
    {
        public static int CategoryId = 1;

        public static List<FinanceCategory> Categories { get; private set; } = new List<FinanceCategory>();

        public static void Add(FinanceCategory financeCategory)
        {
            Categories.Add(financeCategory);
        }

        public static FinanceCategory Get(int id)
        {
            return Categories.Find(x => x.Id == id);
        }

        public static void Sort()
        {
            Categories = Categories.OrderBy(x => x.Id).ToList();
        }

        public static void Remove(int id)
        {
            Categories.RemoveAt(Categories.FindIndex(x => x.Id == id));
        }
    }
}
