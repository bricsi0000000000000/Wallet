using System;
using System.IO;
using System.Linq;
using Wallet.Models;
using Xamarin.Essentials;

namespace Wallet
{
    public static class Database
    {
        private static readonly string settingsFileName = "settings.csv";
        private static readonly string financesFileName = "finances.csv";
        private static readonly string templatesFileName = "templates.csv";
        private static readonly string categoriesFileName = "categories.csv";
        private static readonly string budgetGoalsFileName = "budgetGoals.csv";

        private static void SaveSettings()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, settingsFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath, append: false))
            {
                streamWriter.WriteLine(FinanceManager.InitialMoney);
                streamWriter.WriteLine(SettingsManager.AcitveCurrency.Id);
                streamWriter.WriteLine(SettingsManager.FirstTime);
            }
        }

        public static void SaveInitialMoney()
        {
            SaveSettings();
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

        public static void SaveTemplates()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, templatesFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                foreach (Template template in FinanceManager.Templates)
                {
                    string row = $"{template.Id};{template.Money};{template.Description};{template.CategoryId};{template.Type}";
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

        public static void SaveBudgetGoals()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(documentsPath, budgetGoalsFileName);
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                foreach (Budget budget in BudgetGoalManager.BudgetGoals)
                {
                    string row = $"{budget.Id};{budget.MaxMoney};{budget.SpentMoney};{budget.CategoryId}";
                    streamWriter.WriteLine(row);
                }
            }
        }

        public static void SaveActiveCurrency()
        {
            SaveSettings();
        }

        public static void LoadSettings()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, settingsFileName);

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string[] content = streamReader.ReadToEnd().Split('\n');
                FinanceManager.InitialMoney = int.Parse(content[0]);
                SettingsManager.AcitveCurrency = SettingsManager.Currencies.Find(x => x.Id == int.Parse(content[1]));
                SettingsManager.FirstTime = bool.Parse(content[2]);
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

        public static void LoadTemplates()
        {
            string documentsPath = FileSystem.AppDataDirectory;
            string filePath = Path.Combine(documentsPath, templatesFileName);

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string content = streamReader.ReadToEnd();
                foreach (string row in content.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        string[] data = row.Split(';');
                        Template template = new Template
                        {
                            Id = int.Parse(data[0]),
                            Money = int.Parse(data[1]),
                            Description = data[2],
                            CategoryId = int.Parse(data[3]),
                            Type = (FinanceType)Enum.Parse(typeof(FinanceType), data[4])
                        };

                        FinanceManager.AddTemplate(template);
                    }
                }
            }
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

        public static void LoadBudgetGoals()
        {
            BudgetGoalManager.BudgetGoals.Clear();
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(documentsPath, budgetGoalsFileName);

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string content = streamReader.ReadToEnd();
                foreach (string row in content.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        string[] data = row.Split(';');
                        BudgetGoalManager.Add(new Budget
                        {
                            Id = int.Parse(data[0]),
                            MaxMoney = int.Parse(data[1]),
                            SpentMoney = int.Parse(data[2]),
                            CategoryId = int.Parse(data[3])
                        });
                    }
                }
            }
            BudgetGoalManager.BudgetGoalId = BudgetGoalManager.BudgetGoals.Last().Id + 1;
        }

        public static void ResetDatabase()
        {
            FinanceManager.Finances.Clear();
            SaveFinances();

            FinanceCategoryManager.Categories.Clear();
            FinanceCategoryManager.Add(new FinanceCategory { Id = 1, Name = "Grocery", ColorCode = "#fcc335" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 2, Name = "Restaurant", ColorCode = "#18ce88" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 3, Name = "Transport", ColorCode = "#d8d8d8" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 4, Name = "Shopping", ColorCode = "#fc0591" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 5, Name = "Drink", ColorCode = "#35d7fc" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 6, Name = "Salary", ColorCode = "#ddf473" });
            SaveCategories();

            SaveActiveCurrency();
        }

        public static void LoadFromDatabase()
        {
            try
            {
                LoadSettings();
            }
            catch (Exception e)
            {
            }


            try
            {
                LoadCategories();
            }
            catch (Exception e)
            {
            }

            try
            {
                LoadBudgetGoals();
            }
            catch (Exception e)
            {
            }

            try
            {
                LoadFinances();
            }
            catch (Exception e)
            {
            }

            try
            {
                LoadTemplates();
            }
            catch (Exception e)
            {
            }

            //LoadDefaults();
        }

        public static void LoadDefaults()
        {
           
        }
    }
}
