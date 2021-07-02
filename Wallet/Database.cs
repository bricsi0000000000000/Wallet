﻿using System;
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
                LoadFinances();
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
                LoadSettings();
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

            //LoadDefaults();
        }

        public static void LoadDefaults()
        {
            FinanceCategoryManager.Add(new FinanceCategory { Id = 1, Name = "Grocery", ColorCode = "#fcc335" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 2, Name = "Restaurant", ColorCode = "#18ce88" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 3, Name = "Transport", ColorCode = "#424242" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 4, Name = "Shopping", ColorCode = "#fc0591" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 5, Name = "Drink", ColorCode = "#35d7fc" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 6, Name = "Scholarship", ColorCode = "#f4c773" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 7, Name = "Salary", ColorCode = "#ddf473" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 8, Name = "Deposit", ColorCode = "#2add1a" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 9, Name = "Subscriptions", ColorCode = "#9d59f7" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 10, Name = "Work", ColorCode = "#ddcd1a" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 11, Name = "Entertainment", ColorCode = "#1a55dd" });
            FinanceCategoryManager.Add(new FinanceCategory { Id = 12, Name = "Random income", ColorCode = "#a6c0fc" });
            SaveCategories();

            FinanceManager.Add(new Finance { Id = 34, Description = "Vonatjegy", Date = new DateTime(2021, 07, 2), Money = 1481, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });

            FinanceManager.Add(new Finance { Id = 33, Description = "Asetto Corsa", Date = new DateTime(2021, 06, 02), Money = 7288, Type = FinanceType.Expense, CategoryId = 4, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 32, Description = "Wamus", Date = new DateTime(2021, 06, 07), Money = 927, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 31, Description = "Bus", Date = new DateTime(2021, 06, 07), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 30, Description = "Bus", Date = new DateTime(2021, 06, 07), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 29, Description = "Tesco", Date = new DateTime(2021, 06, 07), Money = 209, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 28, Description = "Spar", Date = new DateTime(2021, 06, 07), Money = 1164, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 27, Description = "Elixir kv", Date = new DateTime(2021, 06, 07), Money = 935, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 26, Description = "Spar", Date = new DateTime(2021, 06, 07), Money = 259, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 25, Description = "Salary", Date = new DateTime(2021, 06, 10), Money = 283921, Type = FinanceType.Income, CategoryId = 7, IsAutomatized = true });
            FinanceManager.Add(new Finance { Id = 24, Description = "Deposit", Date = new DateTime(2021, 06, 10), Money = 280000, Type = FinanceType.Deposit, CategoryId = 8, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 23, Description = "Scholarship", Date = new DateTime(2021, 06, 10), Money = 20700, Type = FinanceType.Income, CategoryId = 6, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 22, Description = "Bus", Date = new DateTime(2021, 06, 15), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 21, Description = "Bus", Date = new DateTime(2021, 06, 15), Money = 119, Type = FinanceType.Expense, CategoryId = 3, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 20, Description = "KFC", Date = new DateTime(2021, 06, 15), Money = 1980, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 19, Description = "Interest", Date = new DateTime(2021, 06, 15), Money = 5, Type = FinanceType.Income, CategoryId = 12, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 18, Description = "Petis döner", Date = new DateTime(2021, 06, 18), Money = 1690, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 17, Description = "Üveges sör", Date = new DateTime(2021, 06, 18), Money = 470, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 16, Description = "Sör", Date = new DateTime(2021, 06, 18), Money = 350, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 15, Description = "Mandala", Date = new DateTime(2021, 06, 18), Money = 500, Type = FinanceType.Expense, CategoryId = 5, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 14, Description = "Chopsticks", Date = new DateTime(2021, 06, 23), Money = 589, Type = FinanceType.Expense, CategoryId = 4, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 35, Description = "Spotify", Date = new DateTime(2021, 06, 27), Money = 916, Type = FinanceType.Expense, CategoryId = 9, IsAutomatized = true });

            FinanceManager.Add(new Finance { Id = 13, Description = "Kisbolt", Date = new DateTime(2021, 05, 3), Money = 872, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 12, Description = "Salary", Date = new DateTime(2021, 05, 10), Money = 262857, Type = FinanceType.Income, CategoryId = 7, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 12, Description = "Scholarship", Date = new DateTime(2021, 05, 10), Money = 20700, Type = FinanceType.Income, CategoryId = 6, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 11, Description = "Deposit", Date = new DateTime(2021, 05, 10), Money = 270000, Type = FinanceType.Deposit, CategoryId = 8, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 10, Description = "Nagybolt", Date = new DateTime(2021, 05, 10), Money = 1605, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 9, Description = "Szünetmentes", Date = new DateTime(2021, 05, 12), Money = 44980, Type = FinanceType.Expense, CategoryId = 4, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 8, Description = "Nagybolt", Date = new DateTime(2021, 05, 17), Money = 1007, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 7, Description = "Allianz transfer credit", Date = new DateTime(2021, 05, 19), Money = 105, Type = FinanceType.Income, CategoryId = 12, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 6, Description = "From deposit", Date = new DateTime(2021, 05, 21), Money = 20000, Type = FinanceType.Income, CategoryId = 12, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 5, Description = "Interest", Date = new DateTime(2021, 05, 21), Money = 1, Type = FinanceType.Income, CategoryId = 12, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 4, Description = "Auchan", Date = new DateTime(2021, 05, 25), Money = 807, Type = FinanceType.Expense, CategoryId = 1, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 3, Description = "MC Donalds", Date = new DateTime(2021, 05, 25), Money = 3160, Type = FinanceType.Expense, CategoryId = 2, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 2, Description = "Interest", Date = new DateTime(2021, 05, 31), Money = 1, Type = FinanceType.Income, CategoryId = 12, IsAutomatized = false });
            FinanceManager.Add(new Finance { Id = 1, Description = "Spotify", Date = new DateTime(2021, 05, 27), Money = 916, Type = FinanceType.Expense, CategoryId = 9, IsAutomatized = false });

            SaveFinances();

            SettingsManager.FirstTime = false;
            SettingsManager.AcitveCurrency = SettingsManager.Currencies[3];
            FinanceManager.InitialMoney = 807938;
            SaveSettings();
        }
    }
}
