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
        private decimal _balance;
        private decimal _expenses;
        private decimal _income;
        #endregion

        #region Properties
        /*
         * DICTIONARY, cat name  + balance
         */
        public ObservableCollection<TransactionType> TransactionTypes { get; set; }

        public IDictionary<TransactionType, decimal> TransactionTypesWithAmounts { get; set; }

        public decimal Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        public decimal Expenses
        {
            get { return _expenses; }
            set { SetProperty(ref _expenses, value); }
        }

        public decimal Income
        {
            get { return _income; }
            set { SetProperty(ref _income, value); }
        }
        #endregion

        #region Constructors
        public InfoViewModel()
        {
            Title = "Info";

            TransactionTypes = new ObservableCollection<TransactionType>();
            TransactionTypesWithAmounts = new Dictionary<TransactionType, decimal>();

            LoadTransactionData();
            LoadTransactionTypes();
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
        #endregion
    }
}
