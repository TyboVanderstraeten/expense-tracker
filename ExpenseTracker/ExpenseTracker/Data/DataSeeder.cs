﻿using ExpenseTracker.Models;
using System;

namespace ExpenseTracker.Data
{
    public class DataSeeder
    {
        #region Methods
        public static void SeedDB()
        {
            Array values = Enum.GetValues(typeof(TransactionType));
            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                App.Database.SaveTransactionAsync(
                    new Transaction(
                        (TransactionType)values.GetValue(random.Next(values.Length)),
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", random.Next(0, 100),
                        DateTime.Now.AddDays(-i)));
            }
        }
        #endregion
    }
}