﻿using SQLite;
using System;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        #region Properties
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        [Ignore]
        public string TextColor {
            get {
                switch (TransactionType)
                {
                    case TransactionType.Income: return "Green";
                    default: return "IndianRed";
                }
            }
        }

        [Ignore]
        public string ImageUrl { get { return $"{TransactionType.ToString().ToUpper()}.png"; } }
        #endregion

        #region Constructors
        public Transaction()
        {

        }

        public Transaction(TransactionType transactionType, string description, decimal amount, DateTime date)
        {
            TransactionType = transactionType;
            Description = description;
            Amount = amount;
            Date = date;
        }
        #endregion
    }
}
