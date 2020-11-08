using ExpenseTracker.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        #region Private fields
        private Month _month = (Month)DateTime.Now.Month;
        private int _year = DateTime.Now.Year;

        private decimal _balance;
        private decimal _expenses;
        private decimal _income;

        private PlotModel _plotModel;
        #endregion

        #region Properties
        public ObservableCollection<Month> Months { get; }
        public ObservableCollection<int> Years { get; }

        public Month Month { get { return _month; } set { SetProperty(ref _month, value); } }
        public int Year { get { return _year; } set { SetProperty(ref _year, value); } }

        public decimal Balance { get { return _balance; } set { SetProperty(ref _balance, value); } }
        public decimal Expenses { get { return _expenses; } set { SetProperty(ref _expenses, value); } }
        public decimal Income { get { return _income; } set { SetProperty(ref _income, value); } }

        public PlotModel PlotModel { get { return _plotModel; } set { SetProperty(ref _plotModel, value); } }
        #endregion

        #region Constructors
        public StatisticsViewModel()
        {
            Months = new ObservableCollection<Month>();
            Years = new ObservableCollection<int>();

            LoadMonths();
            LoadYears();
            FilterData();
        }
        #endregion

        #region Methods
        private void LoadMonths()
        {
            foreach (Month month in Enum.GetValues(typeof(Month)))
            {
                Months.Add(month);
            }
        }

        private void LoadYears()
        {
            for (int i = 2020; i <= DateTime.Now.Year; i++)
            {
                Years.Add(i);
            }
        }

        public async Task FilterData()
        {
            List<Transaction> transactions = await App.Database.GetTransactionsAsync();

            FilterPlotModel(transactions);

            if (Month == Month.All)
            {
                transactions = transactions.Where(t => t.Date.Year == Year).ToList();
            }
            else
            {
                transactions = transactions.Where(t => t.Date.Month == (int)Month && t.Date.Year == Year).ToList();
            }

            Expenses = transactions.Where(t => t.TransactionType != TransactionType.Income).Sum(t => t.Amount);
            Income = transactions.Where(t => t.TransactionType == TransactionType.Income).Sum(t => t.Amount);
            Balance = Income - Expenses;
        }

        private void FilterPlotModel(ICollection<Transaction> transactions)
        {
            PlotModel = new PlotModel() { Background = OxyColors.Transparent };

            CategoryAxis categoryAxisBottom = new CategoryAxis() { Position = AxisPosition.Bottom, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot };
            LinearAxis linearAxis = new LinearAxis() { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot };
            ColumnSeries columnSeries = new ColumnSeries() { IsStacked = true, FillColor = OxyColor.Parse("#734b6d") };

            PlotModel.Axes.Add(categoryAxisBottom);
            PlotModel.Axes.Add(linearAxis);

            if (Month == Month.All)
            {
                PlotModel.Title = $"Top categories per month";

                CategoryAxis categoryAxisTop = new CategoryAxis() { Position = AxisPosition.Top, MajorGridlineStyle = LineStyle.None, MinorGridlineStyle = LineStyle.None };
                PlotModel.Axes.Add(categoryAxisTop);

                var topCategoriesPerMonthForYear = transactions.Where(t => t.Date.Year == Year)
                    .GroupBy(t => t.Date.Month)
                    .Select(g => new
                    {
                        Month = (Month)g.Key,
                        Top = g.Where(t => t.TransactionType != TransactionType.Income)
                        .GroupBy(t => t.TransactionType)
                        .OrderByDescending(g => g.Sum(t => t.Amount))
                        .Select(g => new { TransactionType = g.Key, Amount = g.Sum(t => t.Amount) })
                        .First()
                    })
                    .Select(g => new { g.Month, g.Top.TransactionType, g.Top.Amount })
                    .ToList();

                foreach (Month month in Months.Where(m => m != Month.All))
                {
                    categoryAxisBottom.Labels.Add(month.ToString().Substring(0, 3));

                    if (topCategoriesPerMonthForYear.Any(t => t.Month == month))
                    {
                        var topCategoryForMonth = topCategoriesPerMonthForYear.Single(t => t.Month == month);
                        categoryAxisTop.Labels.Add(topCategoryForMonth.TransactionType.ToString());
                        columnSeries.Items.Add(new ColumnItem(decimal.ToDouble(topCategoryForMonth.Amount)));
                    }
                    else
                    {
                        categoryAxisTop.Labels.Add(string.Empty);
                        columnSeries.Items.Add(new ColumnItem(0));
                    }
                }

                PlotModel.Series.Add(columnSeries);
            }
            else
            {
                PlotModel.Title = $"Categories per month";

                var categoriesForMonth = transactions.Where(t => t.Date.Month == (int)Month && t.Date.Year == Year)
                     .GroupBy(t => t.TransactionType)
                     .Select(g => new { TransactionType = g.Key, Amount = g.Sum(t => t.Amount) })
                     .ToList();

                foreach (var category in categoriesForMonth)
                {
                    categoryAxisBottom.Labels.Add(category.TransactionType.ToString());
                    columnSeries.Items.Add(new ColumnItem(decimal.ToDouble(category.Amount)));
                }

                PlotModel.Series.Add(columnSeries);
            }
        }
        #endregion
    }
}
