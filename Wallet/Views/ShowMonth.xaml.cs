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
    public partial class ShowMonth : ContentPage
    {
        List<Finance> finances;
        List<ChartEntry> expenses = new List<ChartEntry>();
        DateTime date;

        private readonly Color cardBackgroundColor;
        private readonly Color textColor;
        private readonly Color expenseColor;
        private readonly Color incomeColor;

        public ShowMonth(List<Finance> finances, DateTime date)
        {
            InitializeComponent();

            this.finances = finances;
            this.date = date;

            TitleLabel.Text = date.FormatToMonthYear();

            cardBackgroundColor = (Color)Application.Current.Resources["White"];
            textColor = (Color)Application.Current.Resources["Primary"];
            incomeColor = (Color)Application.Current.Resources["Income"];
            expenseColor = (Color)Application.Current.Resources["Expense"];
        }

        protected override void OnAppearing()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            ChartLabels.Children.Clear();

            List<Finance> allFinances = new List<Finance>();

            foreach (IGrouping<int, Finance> group in finances.FindAll(x => x.Type == FinanceType.Expense).GroupBy(x => x.CategoryId))
            {
                Finance finance = new Finance
                {
                    CategoryId = group.Key
                };

                foreach (Finance item in group)
                {
                    finance.Money += item.Money;
                }

                allFinances.Add(finance);
            }

            Sort(allFinances);

            expenses.Clear();

            foreach (Finance finance in allFinances)
            {
                expenses.Add(CreateChartEntry(finance));
            }

            Sort(allFinances, descending: true);

            foreach (Finance finance in allFinances)
            {
                ChartLabels.Children.Add(new ChartValue(finance));
            }

            ExpensesChart.Chart = MakeChart();

            List<Finance> monthlyFinances = FinanceManager.Finances.FindAll(x => x.Date.Month == date.Month && x.Date.Year == date.Year);

            foreach (IGrouping<int, Finance> group in monthlyFinances.GroupBy(x => x.Date.Day))
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
    }
}