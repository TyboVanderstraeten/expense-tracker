using ExpenseTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        #region Private fields
        private readonly InfoViewModel _infoViewModel;
        #endregion

        #region Constructors
        public InfoPage()
        {
            InitializeComponent();

            BindingContext = _infoViewModel = new InfoViewModel();
        }
        #endregion

        #region Methods
        private async void Month_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            await _infoViewModel.OnMonthSelected();
        }

        private void Year_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _infoViewModel.OnYearSelected();
        }
        #endregion
    }
}