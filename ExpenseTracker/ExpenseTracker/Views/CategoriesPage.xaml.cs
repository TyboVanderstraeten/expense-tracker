using ExpenseTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        #region Private fields
        private readonly CategoriesViewModel _categoriesViewModel;
        #endregion

        #region Constructors
        public CategoriesPage()
        {
            InitializeComponent();

            BindingContext = _categoriesViewModel = new CategoriesViewModel();

        }
        #endregion

        #region Methods
        private async void FilterData(object sender, System.EventArgs e)
        {
            await _categoriesViewModel.FilterData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _categoriesViewModel.FilterData();
        }
        #endregion
    }
}