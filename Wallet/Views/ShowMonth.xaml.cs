using System;
using Wallet.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowMonth : ContentPage
    {
        private DateTime date;

        public ShowMonth(DateTime date)
        {
            InitializeComponent();

            this.date = date;

            TitleLabel.Text = date.FormatToFullMonth();
        }

        protected override void OnAppearing()
        {
            ListItems.Children.Clear();

            MonthlyFinancesChartCard monthlyFinances = new MonthlyFinancesChartCard(date);
            ListMonthlyFinances listFinances = new ListMonthlyFinances(date);

            monthlyFinances.SetValue(Grid.RowProperty, 0);
            listFinances.SetValue(Grid.RowProperty, 1);

            ListItems.Children.Add(monthlyFinances);
            ListItems.Children.Add(listFinances);
        }
    }
}