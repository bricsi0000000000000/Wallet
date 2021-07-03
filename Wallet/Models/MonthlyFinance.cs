using System;
using System.Collections.Generic;
using System.Linq;

namespace Wallet.Models
{
    public class MonthlyFinance
    {
        public MonthlyFinance(DateTime date)
        {
            Year = date.Year;
            Month = date.Month;
            Finances = new List<Finance>();
            GroupedFinances = new List<Finance>();
        }

        public int Year { get; private set; }
        public int Month { get; private set; }
        public List<Finance> Finances { get; private set; }
        public List<Finance> GroupedFinances { get; private set; }

        public Finance Get(int id)
        {
            return Finances.Find(x => x.Id == id);
        }

        public void Add(Finance finance)
        {
            Finances.Add(finance);

            GroupByCategories();
        }     

        public void Remove(int id)
        {
            Finances.RemoveAt(Finances.FindIndex(x => x.Id == id));

            GroupByCategories();
        }

        private void GroupByCategories()
        {
            GroupedFinances.Clear();
            foreach (IGrouping<int, Finance> group in Finances.FindAll(x => x.Type == FinanceType.Expense).GroupBy(x => x.CategoryId))
            {
                Finance finance = new Finance
                {
                    CategoryId = group.Key
                };

                foreach (Finance item in group)
                {
                    finance.Money += item.Money;
                }

                GroupedFinances.Add(finance);
            }
        }
    }
}
