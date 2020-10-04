using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpenseTracker.ViewModels
{
    public class TransactionsViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<Transaction> Transactions { get; set; }
        public Command LoadTransactionsCommand { get; set; }
        public Command DeleteTransactionCommand { get; set; }
        #endregion

        #region Constructors
        public TransactionsViewModel()
        {
            Title = "Transactions";

            Transactions = new ObservableCollection<Transaction>();

            LoadTransactionsCommand = new Command(async () => await ExecuteLoadTransactionsCommand());
            DeleteTransactionCommand = new Command(async (transaction) => await ExecuteDeleteTransactionCommand((Transaction)transaction));
        }
        #endregion

        #region Methods
        private async Task ExecuteLoadTransactionsCommand()
        {
            IsBusy = true;

            Transactions.Clear();

            List<Transaction> transactions = await App.Database.GetTransactionsAsync();

            foreach (Transaction transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            IsBusy = false;
        }

        private async Task<int> ExecuteDeleteTransactionCommand(Transaction transaction)
        {
            IsBusy = true;

            int result = await App.Database.DeleteTransactionAsync(transaction);

            Transactions.Remove(transaction);

            IsBusy = false;

            return result;
        }

        public async Task<int> SaveTransactionAsync(Transaction transaction)
        {
            IsBusy = true;

            int result = await App.Database.SaveTransactionAsync(transaction);

            Transactions.Add(transaction);

            IsBusy = false;

            return result;
        }
        #endregion
    }
}
