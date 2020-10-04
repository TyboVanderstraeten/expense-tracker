using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionsPage : ContentPage
    {
        #region Private fields
        private readonly TransactionsViewModel _transactionsViewModel;
        #endregion

        #region Constructors
        public TransactionsPage()
        {
            InitializeComponent();

            BindingContext = _transactionsViewModel = new TransactionsViewModel();
        }
        #endregion

        #region Methods
        private async void OnTransactionSelected(object sender, EventArgs args)
        {
            Transaction transaction = (Transaction)sender;
            string title = $"{transaction.TransactionType} {transaction.ExpenseType}";
            string message = $"{transaction.Description} {transaction.Amount} {transaction.Date}";
            await DisplayAlert(title, message, "Cancel");
        }

        private async void AddTransaction_Clicked(object sender,EventArgs args)
        {
            await Navigation.PushAsync(new NewTransactionPage());
        }
        #endregion
    }
}