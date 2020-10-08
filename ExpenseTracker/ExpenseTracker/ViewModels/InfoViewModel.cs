using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExpenseTracker.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        #region Private fields
        private string _month;
        private int _year;

        private decimal _balance;
        private decimal _expenses;
        private decimal _income;
        #endregion

        #region Properties
        /*
         * DICTIONARY, cat name  + balance
         */
        public ObservableCollection<TransactionType> TransactionTypes { get; }
        public ObservableCollection<int> Years { get; }
        public IDictionary<TransactionType, decimal> TransactionTypesWithAmounts { get; }

        public string Month { get { return _month; } set { SetProperty(ref _month, value); } }
        public int Year { get { return _year; } set { SetProperty(ref _year, value); } }
        public decimal Balance { get { return _balance; } set { SetProperty(ref _balance, value); } }
        public decimal Expenses { get { return _expenses; } set { SetProperty(ref _expenses, value); } }
        public decimal Income { get { return _income; } set { SetProperty(ref _income, value); } }
        #endregion

        #region Constructors
        public InfoViewModel()
        {
            Title = "Info";

            TransactionTypes = new ObservableCollection<TransactionType>();
            Years = new ObservableCollection<int>();
            TransactionTypesWithAmounts = new Dictionary<TransactionType, decimal>();

            LoadTransactionData();
            LoadTransactionTypes();
            LoadYears();
        }
        #endregion

        #region Methods
        private async void LoadTransactionData()
        {
            List<Transaction> transactions = await App.Database.GetTransactionsAsync();

            Expenses = transactions.Where(t => t.TransactionType != TransactionType.INCOME).Sum(t => t.Amount);
            Income = transactions.Where(t => t.TransactionType == TransactionType.INCOME).Sum(t => t.Amount);
            Balance = Expenses - Income;
        }

        private async void LoadTransactionTypes()
        {
            List<Transaction> transactions = await App.Database.GetTransactionsAsync();

            foreach (TransactionType transactionType in Enum.GetValues(typeof(TransactionType)))
            {
                TransactionTypes.Add(transactionType);
                TransactionTypesWithAmounts.Add(transactionType, transactions.Where(t => t.TransactionType == transactionType).Sum(t => t.Amount));
            }
        }

        private void LoadYears()
        {
            int currentYear = DateTime.Now.Year;

            for (int i = 0; i <= 20; i++)
            {
                Years.Add(currentYear + i);
            }
        }
        #endregion
    }
}
