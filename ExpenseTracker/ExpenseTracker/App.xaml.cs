using Xamarin.Forms;
using ExpenseTracker.Views;
using ExpenseTracker.Data;

namespace ExpenseTracker
{
    public partial class App : Application
    {
        #region Variables
        private static Database database;
        #endregion

        #region Properties
        public static Database Database {
            get {
                if (database == null)
                {
                    database = new Database();
                }
                return database;
            }
        }
        #endregion

        #region Constructors
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
        #endregion

        #region Methods
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        #endregion
    }
}
