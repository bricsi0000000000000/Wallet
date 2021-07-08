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

        public static void AddSpentMoney(Finance finance)
        {
            Budget budget = BudgetGoals.Find(x => x.CategoryId == finance.CategoryId);
            if (budget != null)
            {
                if (finance.Type == FinanceType.Income)
                {
                    budget.SpentMoney -= finance.Money;
                }
                else
                {
                    budget.SpentMoney += finance.Money;
                }
            }
        }

        public static void UpdateSpentMoney(Finance finance)
        {
            Budget budget = BudgetGoals.Find(x => x.CategoryId == finance.CategoryId);
            if (budget != null)
            {
                if (finance.Type == FinanceType.Income)
                {
                    budget.SpentMoney += finance.Money;
                }
                else
                {
                    budget.SpentMoney -= finance.Money;
                }
            }
        }

        public static void UpdateSpentMoney(Finance finance, int money)
        {
            Budget budget = BudgetGoals.Find(x => x.CategoryId == finance.CategoryId);
            if (budget != null)
            {
                if (finance.Type == FinanceType.Income)
                {
                    budget.SpentMoney += money;
                }
                else
                {
                    budget.SpentMoney -= money;
                }
            }
        }

        public static void Sort()
        {
            BudgetGoals = BudgetGoals.OrderByDescending(x => (float)x.SpentMoney / x.MaxMoney).ToList();
        }

        public static int AllBudget
        {
            get
            {
                return BudgetGoals.Sum(x => x.MaxMoney);
            }
        }

        public static int SpentBudget
        {
            get
            {
                return BudgetGoals.Sum(x => x.SpentMoney);
            }
        }

        public static int LeftBudget
        {
            get
            {
                return AllBudget - SpentBudget;
            }
        }
    }
}
