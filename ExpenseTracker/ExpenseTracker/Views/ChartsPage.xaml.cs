using ExpenseTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartsPage : ContentPage
    {
        #region Private fields
        private readonly ChartsViewModel _chartsViewModel;
        #endregion

        #region Constructors
        public ChartsPage()
        {
            InitializeComponent();

            BindingContext = _chartsViewModel = new ChartsViewModel();
        }
        #endregion

        #region Methods
        private async void FilterData(object sender, System.EventArgs e)
        {
            await _chartsViewModel.FilterData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _chartsViewModel.FilterData();
        }
        #endregion
    }
}