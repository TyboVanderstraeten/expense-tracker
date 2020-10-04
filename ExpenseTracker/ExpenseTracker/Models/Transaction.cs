using SQLite;
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
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public ExpenseType? ExpenseType { get; set; }
        #endregion

        #region Constructors
        public Transaction()
        {

        }

        public Transaction(TransactionType transactionType, string description, double amount, DateTime date, ExpenseType? expenseType = null)
        {
            TransactionType = transactionType;
            Description = description;
            Amount = amount;
            Date = date;
            ExpenseType = expenseType;
        }
        #endregion
    }
}
