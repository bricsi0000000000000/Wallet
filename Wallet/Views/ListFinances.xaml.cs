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

        public ListFinances()
        {
            InitializeComponent();

            //  ResetDatabase();

            LoadFromDatabase();
        }

        private void ResetDatabase()
        {
            FinanceManager.InsertFront(new Finance { Id = -1, Money = 563821, CategoryId = -1, IsExpense = false });
            Database.SaveFinances();
            FinanceCategoryManager.Categories.Clear();
            FinanceCategoryManager.Add(new FinanceCategory { Id = 1, Name = "Work", ColorCode = "#f7ba04" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 2, Name = "Entertainment", ColorCode = "#049ef7" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 3, Name = "Food", ColorCode = "#18ce88" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 4, Name = "Travel", ColorCode = "#d8d8d8" });
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
            MainGrid.Children.Clear();

            MainGrid.WidthRequest = 380;
            MainGrid.Margin = new Thickness(0, 10, 0, 0);

            RowDefinition rowDefinitionChart = new RowDefinition
            {
                Height = new GridLength(300)
            };

            RowDefinition rowDefinitionList = new RowDefinition
            {
                Height = new GridLength(400)
            };

            RowDefinition rowDefinition3 = new RowDefinition();

            MainGrid.RowDefinitions.Add(rowDefinitionChart);
            MainGrid.RowDefinitions.Add(rowDefinitionList);
            MainGrid.RowDefinitions.Add(rowDefinition3);

            LoadFinances();

            LoadChartFrame();

            LoadFinanceFrames();
        }

        private void LoadChartFrame()
        {
            Frame chartFrame = new Frame
            {
                BackgroundColor = Color.FromHex("#303030"),
                CornerRadius = 5,
                HasShadow = true,
                HeightRequest = 300,
                WidthRequest = 310,
                HorizontalOptions = LayoutOptions.Center
            };
            chartFrame.SetValue(Grid.RowProperty, 0);

            Grid chartGrid = new Grid();
            RowDefinition rowDefinition1 = new RowDefinition
            {
                Height = 45
            };
            RowDefinition rowDefinition2 = new RowDefinition();

            Label chartLabel = new Label()
            {
                Text = $"{allExpenses} HUF",
                FontSize = 25,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("#ffffff")
            };

            ChartView expensesChart = MakeChart();

            chartGrid.Children.Add(chartLabel);
            chartGrid.Children.Add(expensesChart);

            chartLabel.SetValue(Grid.RowProperty, 0);
            expensesChart.SetValue(Grid.RowProperty, 1);

            chartGrid.RowDefinitions.Add(rowDefinition1);
            chartGrid.RowDefinitions.Add(rowDefinition2);

            chartFrame.Content = chartGrid;

            MainGrid.Children.Add(chartFrame);
        }

        private void LoadFinances()
        {
            expenses.Clear();
            foreach (IGrouping<int, Finance> group in FinanceManager.Finances.FindAll(x => x.IsExpense).GroupBy(x => x.CategoryId))
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

        private ChartView MakeChart()
        {

            return new ChartView()
            {
                Chart = new DonutChart
                {
                    Entries = expenses,
                    BackgroundColor = SKColor.Parse("#303030"),
                    LabelMode = LabelMode.RightOnly,
                    LabelTextSize = 30,
                    IsAnimated = true
                }
            };
        }

        private void LoadFinanceFrames()
        {
            ScrollView scrollView = new ScrollView();
            scrollView.SetValue(Grid.RowProperty, 1);

            StackLayout mainStackLayout = new StackLayout
            {
                Margin = new Thickness(20, 10, 20, 20)
            };

            foreach (Finance finance in FinanceManager.Finances)
            {
                Frame financeFrame = new Frame
                {
                    BackgroundColor = Color.FromHex("#303030"),
                    CornerRadius = 5,
                    HasShadow = true,
                    Padding = 20
                };

                Grid grid = new Grid();

                ColumnDefinition columnDefinition1 = new ColumnDefinition
                {
                    Width = new GridLength(20, GridUnitType.Absolute)
                };

                ColumnDefinition columnDefinition2 = new ColumnDefinition
                {
                };

                ColumnDefinition columnDefinition3 = new ColumnDefinition
                {
                    Width = new GridLength(50, GridUnitType.Absolute)
                };

                Label arrowLabel = new Label()
                {
                    Text = finance.IsExpense ? "▼" : "▲",
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = finance.IsExpense ? Color.FromHex("#ea281e") : Color.FromHex("#04f440")
                };

                Label moneyLabel = new Label()
                {
                    Text = finance.Money.ToString() + " HUF",
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("#ffffff")
                };

                Button deleteButton = new Button
                {
                    ClassId = finance.Id.ToString(),
                    Text = "✖",
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("#d64c22"),
                    BackgroundColor = Color.FromHex("#303030"),
                };

                if (finance.Id == -1)
                {
                    arrowLabel.Text = "";
                    arrowLabel.TextColor = Color.FromHex("#ffffff");

                    deleteButton.Text = "";
                }
                else
                {
                    deleteButton.Clicked += Delete_Clicked;
                }

                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.ColumnDefinitions.Add(columnDefinition2);
                grid.ColumnDefinitions.Add(columnDefinition3);

                grid.Children.Add(arrowLabel);
                grid.Children.Add(moneyLabel);
                grid.Children.Add(deleteButton);

                arrowLabel.SetValue(Grid.ColumnProperty, 0);
                moneyLabel.SetValue(Grid.ColumnProperty, 1);
                deleteButton.SetValue(Grid.ColumnProperty, 2);

                financeFrame.Content = grid;

                mainStackLayout.Children.Add(financeFrame);
            }

            scrollView.Content = mainStackLayout;

            MainGrid.Children.Add(scrollView);
        }

        private ChartEntry CreateChartEntry(Finance finance)
        {
            return new ChartEntry(finance.Money)
            {
                Label = FinanceCategoryManager.Get(finance.CategoryId).Name,
                ValueLabel = finance.Money.ToString(),
                Color = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode),
                TextColor = SKColor.Parse("#ffffff"),
                ValueLabelColor = SKColor.Parse(FinanceCategoryManager.Get(finance.CategoryId).ColorCode)
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