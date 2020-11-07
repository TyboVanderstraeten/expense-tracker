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
        #endregion
    }
}