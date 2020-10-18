using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private async void ButtonFilter_Clicked(object sender, System.EventArgs e)
        {
            await _transactionsViewModel.FilterData();
        }

        private async void ButtonAddTransaction_Clicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewTransactionPage(_transactionsViewModel), true);
        }

        private async void ButtonDeleteTransaction_Clicked(object sender, EventArgs args)
        {
            if (CollectionViewTransactions.SelectedItems.Count != 0)
            {
                List<Transaction> transactions = CollectionViewTransactions.SelectedItems.Cast<Transaction>().ToList();

                bool result = await DisplayAlert("Confirmation", "Are you sure you want to delete these transactions?", "Yes", "No");

                if (result)
                {
                    foreach (Transaction transaction in transactions)
                    {
                        await _transactionsViewModel.DeleteTransaction(transaction);
                    }

                    await _transactionsViewModel.FilterData();
                }

                CollectionViewTransactions.SelectedItems.Clear();
            }
            else
            {
                await DisplayAlert("Info", "No transactions selected!", "Cancel");
            }
        }
        #endregion
    }
}