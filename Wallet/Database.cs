using System;
using System.IO;
using System.Linq;
using Wallet.Models;
using Xamarin.Essentials;

namespace Wallet
{
    public static class Database
    {
        private static readonly string initialMoneyFileName = "initialMoney.csv";
        private static readonly string financesFileName = "finances.csv";
        private static readonly string categoriesFileName = "categories.csv";

        public static void SaveInitialMoney()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, initialMoneyFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath, append: false))
            {
                streamWriter.WriteLine(FinanceManager.InitialMoney);
            }
        }

        public static void SaveFinances()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, financesFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                foreach (Finance finance in FinanceManager.Finances)
                {
                    string row = $"{finance.Id};{finance.Money};{finance.Description};{finance.CategoryId};{finance.Type};{finance.Date.ToShortDateString()};{finance.IsAutomatized}";
                    streamWriter.WriteLine(row);
                }
            }
        }

        public static void SaveCategories()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(documentsPath, categoriesFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                foreach (FinanceCategory category in FinanceCategoryManager.Categories)
                {
                    string row = $"{category.Id};{category.Name};{category.ColorCode}";
                    streamWriter.WriteLine(row);
                }
            }
        }

        public static void LoadInitialMoney()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, initialMoneyFileName);

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                FinanceManager.InitialMoney = int.Parse(streamReader.ReadToEnd());
            }
        }

        public static void LoadFinances()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, financesFileName);

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string content = streamReader.ReadToEnd();
                foreach (string row in content.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        string[] data = row.Split(';');
                        Finance finance = new Finance
                        {
                            Id = int.Parse(data[0]),
                            Money = int.Parse(data[1]),
                            Description = data[2],
                            CategoryId = int.Parse(data[3]),
                            Type = (FinanceType)Enum.Parse(typeof(FinanceType), data[4]),
                            Date = DateTime.Parse(data[5]),
                            IsAutomatized = bool.Parse(data[6])
                        };

                        FinanceManager.Add(finance);
                    }
                }
            }
            FinanceManager.FinanceId = FinanceManager.Finances.Last().Id + 1;
        }

        public static void LoadCategories()
        {
            FinanceCategoryManager.Categories.Clear();
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(documentsPath, categoriesFileName);

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string content = streamReader.ReadToEnd();
                foreach (string row in content.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        string[] data = row.Split(';');
                        FinanceCategoryManager.Add(new FinanceCategory
                        {
                            Id = int.Parse(data[0]),
                            Name = data[1],
                            ColorCode = data[2]
                        });
                    }
                }
            }
            FinanceCategoryManager.CategoryId = FinanceCategoryManager.Categories.Last().Id + 1;
        }

        public static void ResetDatabase()
        {
            FinanceManager.Finances.Clear();
            Database.SaveFinances();

            FinanceCategoryManager.Categories.Clear();
            FinanceCategoryManager.Add(new FinanceCategory { Id = 1, Name = "Grocery", ColorCode = "#fcc335" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 2, Name = "Restaurant", ColorCode = "#18ce88" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 3, Name = "Transport", ColorCode = "#d8d8d8" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 4, Name = "Shopping", ColorCode = "#fc0591" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 5, Name = "Drink", ColorCode = "#35d7fc" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 6, Name = "Scholarship", ColorCode = "#f4c773" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 7, Name = "Salary", ColorCode = "#ddf473" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 8, Name = "Deposit", ColorCode = "#262626" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 9, Name = "Subscriptions", ColorCode = "#9d59f7" });
            Database.SaveCategories();
        }
    }
}
