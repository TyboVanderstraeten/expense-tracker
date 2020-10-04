using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTransactionPage : ContentPage
    {
        #region Private fields
        private readonly TransactionsViewModel _transactionsViewModel;
        #endregion

        #region Constructors
        public NewTransactionPage()
        {
            InitializeComponent();

            BindingContext = _transactionsViewModel = new TransactionsViewModel();
        }
        #endregion

        #region Properties
        private async void Save_Clicked(object sender, EventArgs args)
        {
            if (PickerTransactionType.SelectedItem == null || EditorDescription.Text == null || EntryAmount.Text == null || DatePickerDate.Date == null)
            {
                await DisplayAlert("Info", "All fields are required!", "Cancel");
            }
            else
            {
                Transaction transaction = new Transaction(
                        (TransactionType)PickerTransactionType.SelectedItem, EditorDescription.Text, Convert.ToDouble(EntryAmount.Text), DatePickerDate.Date
                    );

                await _transactionsViewModel.SaveTransactionAsync(transaction);
                await Navigation.PopAsync();
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
        #endregion
    }
}