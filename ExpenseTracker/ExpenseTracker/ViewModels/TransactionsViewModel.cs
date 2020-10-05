using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModels
{
    public class TransactionsViewModel : BaseViewModel
    {
        #region Private fields
        private decimal _balance;
        #endregion

        #region Properties
        public ObservableCollection<Transaction> Transactions { get; set; }

        public ICollection<TransactionType> TransactionTypes { get; set; }

        public decimal Balance {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }
        #endregion

        #region Constructors
        public TransactionsViewModel()
        {
            Title = "Transactions";

            Transactions = new ObservableCollection<Transaction>();
            TransactionTypes = new List<TransactionType>();

            SeedTransactionTypes();
            LoadTransactions();
        }
        #endregion

        #region Methods
        private void SeedTransactionTypes()
        {
            foreach (TransactionType transactionType in Enum.GetValues(typeof(TransactionType)))
            {
                TransactionTypes.Add(transactionType);
            }
        }

        private async void LoadTransactions()
        {
            Transactions.Clear();

            List<Transaction> transactions = await App.Database.GetTransactionsAsync();

            foreach (Transaction transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            CalculateBalance();
        }

        private void CalculateBalance()
        {
            decimal expenseAmount = Transactions.Where(t => t.TransactionType != TransactionType.INCOME).Sum(t => t.Amount);
            decimal incomeAmount = Transactions.Where(t => t.TransactionType == TransactionType.INCOME).Sum(t => t.Amount);
            Balance = expenseAmount - incomeAmount;
        }

        public async Task<int> SaveTransactionAsync(Transaction transaction)
        {
            int result = await App.Database.SaveTransactionAsync(transaction);

            Transactions.Add(transaction);

            CalculateBalance();

            return result;
        }

        public async Task<int> DeleteTransaction(Transaction transaction)
        {
            int result = await App.Database.DeleteTransactionAsync(transaction);

            Transactions.Remove(transaction);

            CalculateBalance();

            return result;
        }
        #endregion
    }
}
