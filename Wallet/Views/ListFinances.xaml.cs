using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListFinances : ContentPage
    {
        List<ChartEntry> expenses = new List<ChartEntry>();
        private int allExpenses = 0;

        private const string BACKGROUND_COLOR = "#ffffff";
        private const string TEXT_COLOR = "#344955";
        private const string DATE_COLOR = "#232F34";
        private const string GREEN = "#27a555";
        private const string RED = "#B00020";

        public ListFinances()
        {
            InitializeComponent();

            //ResetDatabase();

            LoadFromDatabase();
        }

        private void ResetDatabase()
        {
            FinanceManager.Add(new Finance { Id = -1, Money = 0, CategoryId = -1, IsExpense = false, Date = default });
            Database.SaveFinances();
            FinanceCategoryManager.Categories.Clear();
            FinanceCategoryManager.Add(new FinanceCategory { Id = 1, Name = "Grocery", ColorCode = "#049ef7" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 2, Name = "Restaurant", ColorCode = "#18ce88" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 3, Name = "Transport", ColorCode = "#d8d8d8" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 4, Name = "Shopping", ColorCode = "#fc0591" });
            Database.SaveCategories();
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
        }

        protected override void OnAppearing()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            allExpenses = 0;

            ListItems.Children.Clear();


            int incomes = FinanceManager.Finances.FindAll(x => !x.IsExpense).Sum(x => x.Money);
            int expenses = FinanceManager.Finances.FindAll(x => x.IsExpense).Sum(x => x.Money);
            FinanceManager.Balance = incomes - expenses;

            List<Finance> monthlyExpenses = FinanceManager.Finances.FindAll(x => x.Date.Month == DateTime.Today.Month);

            BalanceLabel.Text = FinanceManager.Balance.ToString();
            MonthlyExpensesLabel.Text = monthlyExpenses.Sum(x => x.Money).ToString();

            LoadFinances(monthlyExpenses);

            ExpensesChart.Chart = MakeChart();

            LoadFinanceFrames();
        }

        private void LoadFinances(List<Finance> monthlyExpenses)
        {
            expenses.Clear();
            foreach (IGrouping<int, Finance> group in monthlyExpenses.FindAll(x => x.IsExpense).GroupBy(x => x.CategoryId))
            {
                Finance finance = new Finance
                {
                    CategoryId = group.Key
                };

                foreach (Finance item in group)
                {
                    finance.Money += item.Money;
                }

                allExpenses += finance.Money;
                expenses.Add(CreateChartEntry(finance));
            }
        }

        private DonutChart MakeChart()
        {
            return new DonutChart
            {
                Entries = expenses,
                BackgroundColor = SKColor.Parse(BACKGROUND_COLOR),
                LabelMode = LabelMode.RightOnly,
                LabelTextSize = 30,
                IsAnimated = true
            };
        }

        private void AddAutomatizedFinances()
        {
            List<Finance> automatizedFinances = FinanceManager.Finances.FindAll(x => x.IsAutomatized);
            DateTime today = DateTime.Today;

            foreach (Finance finance in automatizedFinances)
            {
                int monthDifference = ((today.Year - finance.Date.Year) * 12) + today.Month - finance.Date.Month;
                for (int i = 1; i < monthDifference; i++)
                {
                    Finance newFinance = new Finance
                    {
                        Id = FinanceManager.FinanceId++,
                        Money = finance.Money,
                        CategoryId = finance.CategoryId,
                        IsExpense = finance.IsExpense,
                        Date = finance.Date.AddMonths(1)
                    };
                    FinanceManager.Add(finance);
                }
            }
        }

        private void LoadFinanceFrames()
        {
            AddAutomatizedFinances();

            FinanceManager.Sort();

            foreach (Finance finance in FinanceManager.Finances)
            {
                if (finance.Id == -1)
                {
                    continue;
                }

                Frame financeFrame = new Frame
                {
                    CornerRadius = 5,
                    HasShadow = true,
                    Padding = 20,
                    WidthRequest = 1000
                };

                Grid grid = new Grid();

                ColumnDefinition columnDefinition1 = new ColumnDefinition();

                ColumnDefinition columnDefinition2 = new ColumnDefinition
                {
                    Width = new GridLength(50, GridUnitType.Absolute)
                };

                StackLayout stackLayout = new StackLayout();

                Label moneyLabel = new Label()
                {
                    Text = finance.Money.ToString() + " HUF",
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center
                };

                TapGestureRecognizer moneyLabelTap = new TapGestureRecognizer();
                moneyLabelTap.Tapped += (s, e) =>
                {
                    Navigation.PushAsync(new AddFinance(finance.Id));
                };
                moneyLabel.GestureRecognizers.Add(moneyLabelTap);

                Label dateLabel = new Label()
                {
                    Text = $"{finance.Date.ToShortDateString()}    {(finance.IsAutomatized ? "Regular" : "")}",
                    FontSize = 15,
                    VerticalOptions = LayoutOptions.Center
                };

                Button deleteButton = new Button
                {
                    ClassId = finance.Id.ToString(),
                    Text = "✖",
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex(RED)
                };

                if (finance.IsExpense)
                {
                    financeFrame.BackgroundColor = Color.FromHex(BACKGROUND_COLOR);
                    deleteButton.BackgroundColor = Color.FromHex(BACKGROUND_COLOR);
                    moneyLabel.TextColor = Color.FromHex(TEXT_COLOR);
                    dateLabel.TextColor = Color.FromHex(DATE_COLOR);
                }
                else
                {
                    financeFrame.BackgroundColor = Color.FromHex(GREEN);
                    deleteButton.BackgroundColor = Color.FromHex(GREEN);
                    moneyLabel.TextColor = Color.FromHex(BACKGROUND_COLOR);
                    dateLabel.TextColor = Color.FromHex(BACKGROUND_COLOR);
                }

                financeFrame.BorderColor = Color.FromHex(FinanceCategoryManager.Get(finance.CategoryId).ColorCode);
                deleteButton.Clicked += Delete_Clicked;
                stackLayout.Children.Add(dateLabel);

                stackLayout.Children.Add(moneyLabel);

                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.ColumnDefinitions.Add(columnDefinition2);

                grid.Children.Add(stackLayout);
                grid.Children.Add(deleteButton);

                stackLayout.SetValue(Grid.ColumnProperty, 0);
                deleteButton.SetValue(Grid.ColumnProperty, 1);

                financeFrame.Content = grid;
                ListItems.Children.Add(financeFrame);
            }
        }

        private ChartEntry CreateChartEntry(Finance finance)
        {
            return new ChartEntry(finance.Money)
            {
                Label = FinanceCategoryManager.Get(finance.CategoryId).Name,
                ValueLabel = finance.Money.ToString(),
                Color = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode),
                TextColor = SKColor.Parse(TEXT_COLOR),
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