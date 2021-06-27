using Microcharts;
using SkiaSharp;
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

        private const string CARD_BACKGROUND_COLOR = "#ffffff";
        private const string TEXT_COLOR = "#344955";

        public ShowMonth(List<Finance> finances)
        {
            InitializeComponent();

            this.finances = finances;
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

            foreach (Finance finance in finances)
            {
                ListItems.Children.Add(new FinanceCard(finance));
            }
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