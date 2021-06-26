using System;
using System.IO;
using System.Linq;
using Wallet.Models;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;

namespace Wallet
{
    public static class Database
    {
        private static readonly string financesFileName = "text.csv";
        private static readonly string categoriesFileName = "categories.csv";

        public static void SaveFinances()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, financesFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                foreach (Finance finance in FinanceManager.Finances)
                {
                    string row = $"{finance.Id};{finance.Money};{finance.CategoryId};{finance.IsExpense};{finance.Date.ToShortDateString()};{finance.IsAutomatized}";
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
                            CategoryId = int.Parse(data[2]),
                            IsExpense = data[3].Equals("True"),
                            Date = DateTime.Parse(data[4]),
                            IsAutomatized = bool.Parse(data[5])
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
    }
}
