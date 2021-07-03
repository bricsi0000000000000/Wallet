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
        }

        protected override void OnAppearing()
        {
            ListItems.Children.Clear();

            ListItems.Children.Add(new MonthlyFinancesChartCard(date));
            ListItems.Children.Add(new ListMonthlyFinances(date));
        }
    }
}