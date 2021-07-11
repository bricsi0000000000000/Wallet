using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Models;
using Wallet.Views;

namespace Wallet
{
    public static class FinanceManager
    {
        public static int FinanceId = 1;
        public static int NetWorth = 0;
        public static int InitialMoney = 0;

        public static List<Finance> Finances { get; private set; } = new List<Finance>();
        public static List<Template> Templates { get; private set; } = new List<Template>();
        public static List<Finance> RegularFinances => Finances.FindAll(x => x.IsAutomatized);

        public static List<MonthlyFinance> MonthlyFinances = new List<MonthlyFinance>();

        public static void Initialize()
        {
            int incomes = Finances.FindAll(x => x.Type == FinanceType.Income).Sum(x => x.Money);
            int expenses = Finances.FindAll(x => x.Type == FinanceType.Expense).Sum(x => x.Money);
            NetWorth = InitialMoney + incomes - expenses;

            LoadMonthlyFinances();
            CalculateBudgetGoals();
        }

        private static void CalculateBudgetGoals()
        {
            foreach (Budget budget in BudgetGoalManager.BudgetGoals)
            {
                int categoryMonthlyMoney = Finances.FindAll(x => x.CategoryId == budget.CategoryId &&
                                                                   x.Date.Year == DateTime.Today.Year &&
                                                                   x.Date.Month == DateTime.Today.Month).Sum(x => x.Money);
                budget.SpentMoney = categoryMonthlyMoney;
            }
        }

        public static void AddAutomatizedFinances()
        {
            List<Finance> automatizedFinances = Finances.FindAll(x => x.IsAutomatized);
            DateTime today = DateTime.Today;

            foreach (Finance finance in automatizedFinances)
            {
                int monthDifference = ((today.Year - finance.Date.Year) * 12) + today.Month - finance.Date.Month;
                for (int i = 0; i < monthDifference; i++)
                {
                    if (today.Day >= finance.Date.Day)
                    {
                        Finance newFinance = new Finance
                        {
                            Id = FinanceId++,
                            Money = finance.Money,
                            Description = finance.Description,
                            CategoryId = finance.CategoryId,
                            Type = finance.Type,
                            Date = finance.Date.AddMonths(1),
                            IsAutomatized = true
                        };
                        Add(newFinance);
                        finance.IsAutomatized = false;

                        Database.SaveFinances();
                    }
                }
            }
        }

        public static void LoadMonthlyFinances()
        {
            Sort();
            MonthlyFinances.Clear();
            foreach (Finance finance in Finances)
            {
                MonthlyFinance monthlyFinance = GetMonthlyFinance(finance.Date);
                if (monthlyFinance == null)
                {
                    MonthlyFinances.Add(new MonthlyFinance(finance.Date));
                }
            }

            foreach (Finance finance in Finances)
            {
                AddMonthlyFinance(finance);
            }
        }

        private static void AddMonthlyFinance(Finance finance)
        {
            GetMonthlyFinance(finance.Date).Add(finance);
        }

        public static MonthlyFinance GetMonthlyFinance(DateTime date)
        {
            return MonthlyFinances.Find(x => x.Year == date.Year && x.Month == date.Month);
        }

        public static void Add(Finance finance)
        {
            BudgetGoalManager.AddSpentMoney(finance);
            Finances.Add(finance);
        }

        public static Finance Get(int id)
        {
            return Finances.Find(x => x.Id == id);
        }

        public static void Remove(int id)
        {
            BudgetGoalManager.UpdateSpentMoney(Get(id));
            Finances.RemoveAt(Finances.FindIndex(x => x.Id == id));
        }

        public static void Sort()
        {
            Finances = Finances.OrderByDescending(x => x.Date).ToList();
        }

        public static bool AddTemplate(Template template)
        {
            foreach (Template item in Templates)
            {
                if (item.Equals(template))
                {
                    return false;
                }
            }

            Templates.Add(template);

            return true;
        }

        public static Template GetTemplate(int id)
        {
            return Templates.Find(x => x.Id == id);
        }

        public static void RemoveTemplate(int id)
        {
            Templates.RemoveAt(Templates.FindIndex(x => x.Id == id));
        }
    }
}
