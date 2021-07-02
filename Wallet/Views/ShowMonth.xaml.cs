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
        private List<Finance> finances;
        private List<ChartEntry> expenses = new List<ChartEntry>();
        private DateTime date;

        private List<Grid> dailyFinanceLabelGrids = new List<Grid>();
        private List<FinanceCard> financeCards = new List<FinanceCard>();

        public ShowMonth(List<Finance> finances, DateTime date)
        {
            InitializeComponent();

            this.finances = finances;
            this.date = date;

            TitleLabel.Text = date.FormatToMonthYear();
        }

        protected override void OnAppearing()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            ListItems.Children.Clear();
            ChartStackLayout.Children.Clear();

            dailyFinanceLabelGrids.Clear();
            financeCards.Clear();

            List<Finance> groupedFinances = new List<Finance>();

            foreach (IGrouping<int, Finance> group in finances.FindAll(x => x.Type == FinanceType.Expense).GroupBy(x => x.CategoryId))
            {
                Finance finance = new Finance
                {
                    CategoryId = group.Key,
                    Date = group.First().Date
                };

                foreach (Finance item in group)
                {
                    finance.Money += item.Money;
                }

                groupedFinances.Add(finance);
            }

            Sort(groupedFinances);

            expenses.Clear();

            foreach (Finance finance in groupedFinances)
            {
                expenses.Add(CreateChartEntry(finance));
            }

            Sort(groupedFinances, descending: true);

            ChartStackLayout.Children.Add(new MonthlyFinancesChartCard(groupedFinances, expenses, date));

            List<Finance> monthlyFinances = FinanceManager.Finances.FindAll(x => x.Date.Month == date.Month && x.Date.Year == date.Year);

            foreach (IGrouping<int, Finance> group in monthlyFinances.GroupBy(x => x.Date.Day))
            {
                Grid grid = new Grid
                {
                    ClassId = group.First().Date.Day.ToString()
                };
                ColumnDefinition columnDefinition1 = new ColumnDefinition();
                ColumnDefinition columnDefinition2 = new ColumnDefinition();

                Label dateLabel = new Label
                {
                    Text = group.First().Date.FormatToDate(),
                    TextColor = ColorManager.Text,
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
                    TextColor = ColorManager.ExpenseOrIncome(money < 0),
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

                dailyFinanceLabelGrids.Add(grid);
                ListItems.Children.Add(grid);

                foreach (Finance finance in group)
                {
                    FinanceCard financeCard = new FinanceCard(finance);
                    financeCards.Add(financeCard);
                    ListItems.Children.Add(financeCard);
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

        private ChartEntry CreateChartEntry(Finance finance)
        {
            return new ChartEntry(finance.Money)
            {
                //Label = FinanceCategoryManager.Get(finance.CategoryId).Name,
                //ValueLabel = finance.Money.ToString(),
                Color = FinanceCategoryManager.Get(finance.CategoryId).ColorCode.ToSKColor(),
                //TextColor = SKColor.Parse(textColor.ToHex()),
                //TextColor = SKColor.Parse(cardBackgroundColor.ToHex()),
                //ValueLabelColor = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode)
                //ValueLabelColor = SKColor.Parse(cardBackgroundColor.ToHex())
            };
        }
    }
}