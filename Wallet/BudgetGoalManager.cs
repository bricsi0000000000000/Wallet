using System.Collections.Generic;
using System.Linq;
using Wallet.Models;

namespace Wallet
{
    public static class BudgetGoalManager
    {
        public static int BudgetGoalId = 1;

        public static List<Budget> BudgetGoals { get; private set; } = new List<Budget>();

        public static void Add(Budget budget)
        {
            BudgetGoals.Add(budget);
        }

        public static Budget Get(int id)
        {
            return BudgetGoals.Find(x => x.Id == id);
        }

        public static void Remove(int id)
        {
            BudgetGoals.RemoveAt(BudgetGoals.FindIndex(x => x.Id == id));
        }

        public static void Sort()
        {
            BudgetGoals = BudgetGoals.OrderByDescending(x => (float)x.SpentMoney / x.MaxMoney).ToList();
        }
    }
}
