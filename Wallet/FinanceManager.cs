﻿using System.Collections.Generic;
using Wallet.Models;

namespace Wallet
{
    public static class FinanceManager
    {
        public static int FinanceId = 1;

        public static List<Finance> Finances { get; private set; } = new List<Finance>();

        public static void InsertFront(Finance finance)
        {
            Finances.Insert(0, finance);
        }

        public static void Add(Finance finance)
        {
            Finances.Add(finance);
        }

        public static Finance Get(int id)
        {
            return Finances.Find(x => x.Id == id);
        }

        public static void Remove(int id)
        {
            Finances.RemoveAt(Finances.FindIndex(x => x.Id == id));
        }
    }
}
