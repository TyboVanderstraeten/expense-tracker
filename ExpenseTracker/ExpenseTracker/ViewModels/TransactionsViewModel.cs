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
        private Month _month = Month.ALL;
        private int _year = DateTime.Now.Year;

        private decimal _balance;
        private decimal _expenses;
        private decimal _income;
        #endregion

        #region Properties
        public ObservableCollection<Month> Months { get; }
        public ObservableCollection<int> Years { get; }
        public ObservableCollection<Transaction> Transactions { get; set; }


        public Month Month { get { return _month; } set { SetProperty(ref _month, value); } }
        public int Year { get { return _year; } set { SetProperty(ref _year, value); } }

        public decimal Balance { get { return _balance; } set { SetProperty(ref _balance, value); } }
        public decimal Expenses { get { return _expenses; } set { SetProperty(ref _expenses, value); } }
        public decimal Income { get { return _income; } set { SetProperty(ref _income, value); } }
        #endregion

        #region Constructors
        public TransactionsViewModel()
        {
            Title = "Transactions";

            Months = new ObservableCollection<Month>();
            Years = new ObservableCollection<int>();
            Transactions = new ObservableCollection<Transaction>();

            LoadMonths();
            LoadYears();
            FilterTransactions().Wait();
        }
        #endregion

        #region Methods
        private void LoadMonths()
        {
            foreach (Month month in Enum.GetValues(typeof(Month)))
            {
                Months.Add(month);
            }
        }

        private void LoadYears()
        {
            for (int i = 2020; i <= DateTime.Now.Year; i++)
            {
                Years.Add(i);
            }
        }

        public async Task FilterTransactions()
        {
            List<Transaction> transactions = await App.Database.GetTransactionsAsync();

            if (Month == Month.ALL)
            {
                transactions = transactions.Where(t => t.Date.Year == Year).ToList();
            }
            else
            {
                transactions = transactions.Where(t => t.Date.Month == (int)Month && t.Date.Year == Year).ToList();
            }

            Transactions.Clear();

            foreach(Transaction transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            Expenses = transactions.Where(t => t.TransactionType != TransactionType.INCOME).Sum(t => t.Amount);
            Income = transactions.Where(t => t.TransactionType == TransactionType.INCOME).Sum(t => t.Amount);
            Balance = Expenses - Income;
        }

        public async Task<int> SaveTransactionAsync(Transaction transaction)
        {
            int result = await App.Database.SaveTransactionAsync(transaction);

            Transactions.Add(transaction);

            return result;
        }

        public async Task<int> DeleteTransaction(Transaction transaction)
        {
            int result = await App.Database.DeleteTransactionAsync(transaction);

            Transactions.Remove(transaction);

            return result;
        }
        #endregion
    }
}
