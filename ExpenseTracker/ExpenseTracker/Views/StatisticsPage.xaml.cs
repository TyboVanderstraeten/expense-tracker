using ExpenseTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        #region Private fields
        private readonly StatisticsViewModel _statisticsViewModel;
        #endregion

        #region Constructors
        public StatisticsPage()
        {
            InitializeComponent();

            BindingContext = _statisticsViewModel = new StatisticsViewModel();
        }
        #endregion

        #region Methods
        private async void FilterData(object sender, System.EventArgs e)
        {
            await _statisticsViewModel.FilterData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _statisticsViewModel.FilterData();
        }
        #endregion
    }
}