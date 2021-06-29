using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

            LoadFromDatabase();
        }

        private void LoadFromDatabase()
        {
            try
            {
                Database.LoadFinances();
            }
            catch (Exception e)
            {
            }

            try
            {
                Database.LoadCategories();
            }
            catch (Exception e)
            {
            }

            try
            {
                Database.LoadInitialMoney();
            }
            catch (Exception e)
            {
            }

            //LoadDefaults();
        }

        private void LoadDefaults()
        {
            Database.ResetDatabase();

            FinanceManager.Add(new Finance { Id = 1, Description = "Asetto Corsa", Date = new DateTime(2021, 06, 02), Money = 7288, Type = FinanceType.Expense, CategoryId = 4, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 2, Description = "Wamus", Date = new DateTime(2021, 06, 07), Money = 927, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 3, Description = "Bus", Date = new DateTime(2021, 06, 07), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 4, Description = "Bus", Date = new DateTime(2021, 06, 07), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 5, Description = "Tesco", Date = new DateTime(2021, 06, 07), Money = 209, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 6, Description = "Spar", Date = new DateTime(2021, 06, 07), Money = 1164, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 7, Description = "Elixir kv", Date = new DateTime(2021, 06, 07), Money = 935, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 8, Description = "Spar", Date = new DateTime(2021, 06, 07), Money = 259, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 9, Description = "Salary", Date = new DateTime(2021, 06, 10), Money = 283921, Type = FinanceType.Income, CategoryId = 7, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 10, Description = "Deposit", Date = new DateTime(2021, 06, 10), Money = 280000, Type = FinanceType.Deposit, CategoryId = 8, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 11, Description = "Scholarship", Date = new DateTime(2021, 06, 10), Money = 20700, Type = FinanceType.Income, CategoryId = 6, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 12, Description = "Bus", Date = new DateTime(2021, 06, 15), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 13, Description = "Bus", Date = new DateTime(2021, 06, 15), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 14, Description = "KFC", Date = new DateTime(2021, 06, 15), Money = 1980, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 15, Description = "Petis döner", Date = new DateTime(2021, 06, 21), Money = 1690, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 16, Description = "Üveges sör", Date = new DateTime(2021, 06, 22), Money = 470, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 17, Description = "Sör", Date = new DateTime(2021, 06, 22), Money = 350, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 18, Description = "Chopsticks", Date = new DateTime(2021, 06, 23), Money = 589, Type = FinanceType.Expense, CategoryId = 4, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 19, Description = "Spotify", Date = new DateTime(2021, 05, 27), Money = 916, Type = FinanceType.Expense, CategoryId = 9, IsAutomatized = true });
            Database.SaveFinances();
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
        }

        private void LoadFinances(List<Finance> monthlyExpenses)
        {
            expenses.Clear();
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

                expenses.Add(CreateChartEntry(finance));
            }
        }

        private DonutChart MakeChart()
        {
            return new DonutChart
            {
                Entries = expenses,
                BackgroundColor = SKColor.Parse(cardBackgroundColor.ToHex()),
                LabelMode = LabelMode.RightOnly,
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
                TextColor = SKColor.Parse(textColor.ToHex()),
                ValueLabelColor = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode)
            };
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFinance(-1));
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