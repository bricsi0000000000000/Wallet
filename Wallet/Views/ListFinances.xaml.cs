using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListFinances : ContentPage
    {
        List<ChartEntry> expenses = new List<ChartEntry>();

        private readonly Color cardBackgroundColor;
        private readonly Color textColor;
        private readonly Color expenseColor;
        private readonly Color incomeColor;

        public ListFinances()
        {
            InitializeComponent();

            cardBackgroundColor = (Color)Application.Current.Resources["White"];
            textColor = (Color)Application.Current.Resources["Primary"];
            incomeColor = (Color)Application.Current.Resources["Income"];
            expenseColor = (Color)Application.Current.Resources["Expense"];

            Database.LoadFromDatabase();
            //Database.SaveBudgetGoals();
        }

        protected override void OnAppearing()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            ListItems.Children.Clear();

            int incomes = FinanceManager.Finances.FindAll(x => x.Type == FinanceType.Income).Sum(x => x.Money);
            int expenses = FinanceManager.Finances.FindAll(x => x.Type == FinanceType.Expense).Sum(x => x.Money);
            int deposits = FinanceManager.Finances.FindAll(x => x.Type == FinanceType.Deposit).Sum(x => x.Money);
            FinanceManager.Balance = FinanceManager.InitialMoney + incomes - expenses - deposits;

            List<Finance> monthlyExpenses = FinanceManager.Finances.FindAll(x => x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year && x.Type == FinanceType.Expense);

            ExpensesChartFrame.IsVisible = monthlyExpenses.Any();

            LoadFinances(monthlyExpenses);

            ExpensesChart.Chart = MakeChart();

            LoadFinanceFrames();
            CalculateBudgetGoals();
        }

        private void CalculateBudgetGoals()
        {
            foreach (Budget budget in BudgetGoalManager.BudgetGoals)
            {
                int categoryMonthlyMoney = FinanceManager.Finances.FindAll(x => x.CategoryId == budget.CategoryId &&
                                                                                       x.Date.Year == DateTime.Today.Year &&
                                                                                       x.Date.Month == DateTime.Today.Month).Sum(x => x.Money);
                budget.SpentMoney = categoryMonthlyMoney;
            }
        }

        private void LoadFinances(List<Finance> monthlyExpenses)
        {
            ChartLabels.Children.Clear();

            List<Finance> finances = new List<Finance>();

            foreach (IGrouping<int, Finance> group in monthlyExpenses.FindAll(x => x.Type == FinanceType.Expense).GroupBy(x => x.CategoryId))
            {
                Finance finance = new Finance
                {
                    CategoryId = group.Key
                };

                foreach (Finance item in group)
                {
                    finance.Money += item.Money;
                }

                finances.Add(finance);
            }

            Sort(finances);

            expenses.Clear();

            foreach (Finance finance in finances)
            {
                expenses.Add(CreateChartEntry(finance));
            }

            Sort(finances, descending: true);

            foreach (Finance finance in finances)
            {
                ChartLabels.Children.Add(new ChartValue(finance));
            }
        }

        private void Sort(List<Finance> finances, bool descending = false)
        {
            for (int j = 0; j < finances.Count - 1; j++)
            {
                for (int i = 0; i < finances.Count - 1; i++)
                {
                    if (descending)
                    {
                        if (finances[i].Money < finances[i + 1].Money)
                        {
                            Finance temp = finances[i + 1];
                            finances[i + 1] = finances[i];
                            finances[i] = temp;
                        }
                    }
                    else
                    {
                        if (finances[i].Money > finances[i + 1].Money)
                        {
                            Finance temp = finances[i + 1];
                            finances[i + 1] = finances[i];
                            finances[i] = temp;
                        }
                    }
                }
            }
        }

        private RadialGaugeChart MakeChart()
        {
            return new RadialGaugeChart
            {
                Entries = expenses,
                BackgroundColor = SKColor.Parse(cardBackgroundColor.ToHex()),
                LabelTextSize = 30,
                IsAnimated = false,
                AnimationDuration = new TimeSpan()
            };
        }

        private void AddAutomatizedFinances()
        {
            List<Finance> automatizedFinances = FinanceManager.Finances.FindAll(x => x.IsAutomatized);
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
                            Id = FinanceManager.FinanceId++,
                            Money = finance.Money,
                            Description = finance.Description,
                            CategoryId = finance.CategoryId,
                            Type = finance.Type,
                            Date = finance.Date.AddMonths(1),
                            IsAutomatized = true
                        };
                        FinanceManager.Add(newFinance);
                        finance.IsAutomatized = false;

                        Database.SaveFinances();
                    }
                }
            }
        }

        private void LoadFinanceFrames()
        {
            AddAutomatizedFinances();

            FinanceManager.Sort();

            foreach (IGrouping<int, Finance> group in FinanceManager.Finances.FindAll(x => x.Date.Year == DateTime.Today.Year && x.Date.Month == DateTime.Today.Month).GroupBy(x => x.Date.Day))
            {
                Grid grid = new Grid();
                ColumnDefinition columnDefinition1 = new ColumnDefinition();
                ColumnDefinition columnDefinition2 = new ColumnDefinition();

                Label dateLabel = new Label
                {
                    Text = group.First().Date.FormatToDate(),
                    TextColor = textColor,
                    FontSize = 15,
                    Margin = new Thickness(20, 0, 20, 0)
                };

                int money = 0;
                foreach (Finance finance in group)
                {
                    if (finance.Type == FinanceType.Expense)
                    {
                        money -= finance.Money;
                    }
                    else if (finance.Type == FinanceType.Income)
                    {
                        money += finance.Money;
                    }
                }

                Label moneyLabel = new Label
                {
                    Text = money.FormatToMoney(),
                    TextColor = money < 0 ? expenseColor : incomeColor,
                    FontSize = 15,
                    HorizontalTextAlignment = TextAlignment.End,
                    Margin = new Thickness(0, 0, 20, 0)
                };

                dateLabel.SetValue(Grid.ColumnProperty, 0);
                moneyLabel.SetValue(Grid.ColumnProperty, 1);

                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.ColumnDefinitions.Add(columnDefinition2);

                grid.Children.Add(dateLabel);
                grid.Children.Add(moneyLabel);

                ListItems.Children.Add(grid);

                foreach (Finance finance in group)
                {
                    ListItems.Children.Add(new FinanceCard(finance));
                }
            }

            Frame emptyFrame = new Frame
            {
                CornerRadius = 5,
                HasShadow = false,
                Padding = 20,
                HeightRequest = 100,
                Opacity = 0
            };

            ListItems.Children.Add(emptyFrame);
        }

        private ChartEntry CreateChartEntry(Finance finance)
        {
            return new ChartEntry(finance.Money)
            {
                Label = FinanceCategoryManager.Get(finance.CategoryId).Name,
                ValueLabel = finance.Money.ToString(),
                Color = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode),
                //TextColor = SKColor.Parse(textColor.ToHex()),
                TextColor = SKColor.Parse(cardBackgroundColor.ToHex()),
                //ValueLabelColor = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode)
                ValueLabelColor = SKColor.Parse(cardBackgroundColor.ToHex())
            };
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFinance());
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int id = int.Parse(button.ClassId);
            FinanceManager.Remove(id);
            Database.SaveFinances();
            LoadUI();
        }
    }
}