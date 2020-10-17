using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

        private IDictionary<TransactionType, decimal> _transactionTypesWithAmounts;
        #endregion

        #region Properties
        public ObservableCollection<TransactionType> TransactionTypes { get; }
        public ObservableCollection<int> Years { get; }
        public IDictionary<TransactionType, decimal> TransactionTypesWithAmounts { get { return _transactionTypesWithAmounts; } set { SetProperty(ref _transactionTypesWithAmounts, value); } }

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

            _transactionTypesWithAmounts = new Dictionary<TransactionType, decimal>();

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
            for (int i = 2020; i <= 2025; i++)
            {
                Years.Add(i);
            }
        }

        public async Task OnMonthSelected()
        {
            List<Transaction> transactions = await App.Database.GetTransactionsAsync();
            int month = Month switch
            {
                "All" => 0,
                "January" => 1,
                "February" => 2,
                "March" => 3,
                "April" => 4,
                "May" => 5,
                "June" => 6,
                "July" => 7,
                "August" => 8,
                "September" => 9,
                "October" => 10,
                "November" => 11,
                "December" => 12,
                _ => throw new NotImplementedException()
            };

            if (month == 0)
            {
                transactions = await App.Database.GetTransactionsAsync();
            }
            else
            {
                transactions = transactions.Where(t => t.Date.Month == month).OrderByDescending(t => t.Date).ToList();
            }

            Expenses = transactions.Where(t => t.TransactionType != TransactionType.INCOME).Sum(t => t.Amount);
            Income = transactions.Where(t => t.TransactionType == TransactionType.INCOME).Sum(t => t.Amount);
            Balance = Expenses - Income;

            TransactionTypesWithAmounts.Clear();

            foreach (TransactionType transactionType in Enum.GetValues(typeof(TransactionType)))
            {
                TransactionTypesWithAmounts.Add(transactionType, transactions.Where(t => t.TransactionType == transactionType).Sum(t => t.Amount));
            }
        }

        public void OnYearSelected()
        {

        }
        #endregion
    }
}
