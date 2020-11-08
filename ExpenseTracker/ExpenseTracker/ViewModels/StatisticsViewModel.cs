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

        private PlotModel _model;
        #endregion

        #region Properties
        public ObservableCollection<Month> Months { get; }
        public ObservableCollection<int> Years { get; }

        public PlotModel Model { get { return _model; } set { SetProperty(ref _model, value); } }

        public Month Month { get { return _month; } set { SetProperty(ref _month, value); } }
        public int Year { get { return _year; } set { SetProperty(ref _year, value); } }
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

            if (Month == Month.All)
            {
                transactions = transactions.Where(t => t.Date.Year == Year).ToList();
            }
            else
            {
                transactions = transactions.Where(t => t.Date.Month == (int)Month && t.Date.Year == Year).ToList();
            }

            List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

            foreach (TransactionType transactionType in Enum.GetValues(typeof(TransactionType)))
            {
                CategoryInfo categoryInfo = new CategoryInfo(transactionType, transactions.Where(t => t.TransactionType == transactionType).Sum(t => t.Amount));
                if (categoryInfo.Amount > 0)
                {
                    categoryInfos.Add(categoryInfo);
                }
            }

            await DrawChart();
        }

        private async Task DrawChart()
        {
            if (Month == Month.All)
            {
                CategoryAxis xaxis = new CategoryAxis();
                xaxis.Position = AxisPosition.Bottom;
                xaxis.MajorGridlineStyle = LineStyle.Solid;
                xaxis.MinorGridlineStyle = LineStyle.Dot;

                CategoryAxis xaxis2 = new CategoryAxis();
                xaxis2.Position = AxisPosition.Top;
                xaxis2.MajorGridlineStyle = LineStyle.None;
                xaxis2.MinorGridlineStyle = LineStyle.None;



                LinearAxis yaxis = new LinearAxis();
                yaxis.Position = AxisPosition.Left;
                yaxis.MajorGridlineStyle = LineStyle.Dot;
                xaxis.MinorGridlineStyle = LineStyle.Dot;

                ColumnSeries s1 = new ColumnSeries();
                s1.IsStacked = true;

                List<Transaction> transactions = await App.Database.GetTransactionsAsync();

                //transactions = transactions.Where(t => t.Date.Year == Year).ToList();

                var topCategoryPerMonthByYear =
                     App.Database.GetTransactionsAsync()
                     .Result
                    .Where(t => t.Date.Year == Year)
                    .GroupBy(t => t.Date.Month).Select(g => new
                    {
                        Month = (Month)g.Key,
                        Top =
                        g.Where(t => t.TransactionType != TransactionType.Income)
                        .GroupBy(t => t.TransactionType)
                        .OrderByDescending(g => g.Sum(t => t.Amount))
                        .Select(g => new { Type = g.Key, Amount = g.Sum(t => t.Amount) })
                        .First()
                    })
                    .Select(g => new { g.Month, g.Top.Type, g.Top.Amount })
                    .ToList();

                foreach (Month month in Months.Where(m=>m!=Month.All))
                {
                    if (topCategoryPerMonthByYear.Any(t => t.Month == month))
                    {
                        var t = topCategoryPerMonthByYear.Single(t => t.Month == month);
                        xaxis.Labels.Add(month.ToString().Substring(0, 3));
                        xaxis2.Labels.Add(t.Type.ToString());
                        s1.Items.Add(new ColumnItem(decimal.ToDouble(t.Amount)));
                        
                    }
                    else
                    {
                        xaxis.Labels.Add(month.ToString().Substring(0, 3));
                        xaxis2.Labels.Add("");
                        s1.Items.Add(new ColumnItem(0));
                    }
                }


                Model = new PlotModel();
                Model.Title = $"Top categories per month";
                Model.Background = OxyColors.Transparent;

                Model.Axes.Add(xaxis);
                Model.Axes.Add(xaxis2);
                Model.Axes.Add(yaxis);
                Model.Series.Add(s1);
            }
        }
        #endregion
    }
}
