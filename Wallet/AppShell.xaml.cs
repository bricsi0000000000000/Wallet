using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wallet
{
    public partial class AppShell : Shell
    {
        Uri uri;

        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            uri = new Uri("http://richard.bolya.eu/");
            OpenBrowser(uri);
        }

        public async void OpenBrowser(Uri uri)
        {
            await Browser.OpenAsync(uri);
        }
    }
}
