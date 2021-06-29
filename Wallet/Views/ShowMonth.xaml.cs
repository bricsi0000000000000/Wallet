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

        private const string CARD_BACKGROUND_COLOR = "#ffffff";
        private const string TEXT_COLOR = "#344955";
        private const string RED = "#B00020";
        private const string GREEN = "#27a555";

        public ShowMonth(List<Finance> finances, DateTime date)
        {
            InitializeComponent();

            this.finances = finances;
            this.date = date;
        }

        protected override void OnAppearing()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            expenses.Clear();
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

                expenses.Add(CreateChartEntry(finance));
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
                    TextColor = Color.FromHex(TEXT_COLOR),
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
                    TextColor = Color.FromHex(money < 0 ? RED : GREEN),
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

        private DonutChart MakeChart()
        {
            return new DonutChart
            {
                Entries = expenses,
                BackgroundColor = SKColor.Parse(CARD_BACKGROUND_COLOR),
                LabelMode = LabelMode.RightOnly,
                LabelTextSize = 30,
                IsAnimated = false
            };
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
    }
}