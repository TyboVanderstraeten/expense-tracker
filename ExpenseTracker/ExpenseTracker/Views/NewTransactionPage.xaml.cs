﻿using ExpenseTracker.Models;
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
        public NewTransactionPage(TransactionsViewModel transactionsViewModel)
        {
            InitializeComponent();

            BindingContext = _transactionsViewModel = transactionsViewModel;

            EntryDescription.Text = "No description";

            DatePickerDate.MinimumDate = new DateTime(2020, 1, 1);
            DatePickerDate.MaximumDate = new DateTime(DateTime.Now.Year, 12, 31);
        }
        #endregion

        #region Methods
        private async void ButtonCancel_Clicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync(true);
        }

        private async void ButtonSave_Clicked(object sender, EventArgs args)
        {
            if (PickerTransactionType.SelectedItem == null || EntryDescription.Text == null || EntryAmount.Text == null || DatePickerDate.Date == null)
            {

                await DisplayAlert("Info", "All fields are required!", "Cancel");
            }
            else if (Convert.ToDecimal(EntryAmount.Text) <= 0)
            {
                await DisplayAlert("Info", "Amount must be greater than 0!", "Cancel");
            }
            else
            {
                Transaction transaction = new Transaction((TransactionType)PickerTransactionType.SelectedItem, EntryDescription.Text, Convert.ToDecimal(EntryAmount.Text), DatePickerDate.Date);

                await _transactionsViewModel.SaveTransactionAsync(transaction);
                await _transactionsViewModel.FilterData();
                await Navigation.PopAsync(true);
            }
        }
        #endregion
    }
}