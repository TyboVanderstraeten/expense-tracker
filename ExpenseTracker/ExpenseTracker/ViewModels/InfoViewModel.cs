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
        private Month _month = Month.ALL;
        private int _year = DateTime.Now.Year;

        private decimal _balance;
        private decimal _expenses;
        private decimal _income;
        #endregion

        #region Properties
        public ObservableCollection<Month> Months { get; }
        public ObservableCollection<int> Years { get; }
        public ObservableCollection<CategoryInfo> CategoryInfos { get; }

        public Month Month { get { return _month; } set { SetProperty(ref _month, value); } }
        public int Year { get { return _year; } set { SetProperty(ref _year, value); } }

        public decimal Balance { get { return _balance; } set { SetProperty(ref _balance, value); } }
        public decimal Expenses { get { return _expenses; } set { SetProperty(ref _expenses, value); } }
        public decimal Income { get { return _income; } set { SetProperty(ref _income, value); } }
        #endregion

        #region Constructors
        public InfoViewModel()
        {
            Title = "Info";

            Months = new ObservableCollection<Month>();
            Years = new ObservableCollection<int>();
            CategoryInfos = new ObservableCollection<CategoryInfo>();

            LoadMonths();
            LoadYears();
            FilterData();

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

        public async Task FilterData()
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

            Expenses = transactions.Where(t => t.TransactionType != TransactionType.INCOME).Sum(t => t.Amount);
            Income = transactions.Where(t => t.TransactionType == TransactionType.INCOME).Sum(t => t.Amount);
            Balance = Expenses - Income;

            List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

            foreach (TransactionType transactionType in Enum.GetValues(typeof(TransactionType)))
            {
               categoryInfos.Add(new CategoryInfo(transactionType, transactions.Where(t => t.TransactionType == transactionType).Sum(t => t.Amount)));
            }

            CategoryInfos.Clear();

            foreach(CategoryInfo categoryInfo in categoryInfos.OrderByDescending(ci => ci.Amount))
            {
                CategoryInfos.Add(categoryInfo);
            }
        }
        #endregion
    }
}
