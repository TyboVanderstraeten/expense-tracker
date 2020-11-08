﻿using ExpenseTracker.Models;
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

        private PlotModel _model;
        #endregion

        #region Properties
        public ObservableCollection<Month> Months { get; }
        public ObservableCollection<int> Years { get; }

        public Month Month { get { return _month; } set { SetProperty(ref _month, value); } }
        public int Year { get { return _year; } set { SetProperty(ref _year, value); } }

        public decimal Balance { get { return _balance; } set { SetProperty(ref _balance, value); } }
        public decimal Expenses { get { return _expenses; } set { SetProperty(ref _expenses, value); } }
        public decimal Income { get { return _income; } set { SetProperty(ref _income, value); } }

        public PlotModel Model { get { return _model; } set { SetProperty(ref _model, value); } }
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

            Expenses = transactions.Where(t => t.TransactionType != TransactionType.Income).Sum(t => t.Amount);
            Income = transactions.Where(t => t.TransactionType == TransactionType.Income).Sum(t => t.Amount);
            Balance = Income - Expenses;

            await FilterPlotModel();
        }

        private async Task FilterPlotModel()
        {
            CategoryAxis xaxis = new CategoryAxis() { Position = AxisPosition.Bottom, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot };
            LinearAxis yaxis = new LinearAxis() { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot };
            ColumnSeries s1 = new ColumnSeries() { /*LabelFormatString = "{0}", LabelPlacement = LabelPlacement.Middle,*/ IsStacked = true, FillColor = OxyColor.Parse("#734b6d") };
            Model = new PlotModel() { Background = OxyColors.Transparent };
            Model.Axes.Add(xaxis);
            Model.Axes.Add(yaxis);

            if (Month == Month.All)
            {
                Model.Title = $"Top categories per month";

                CategoryAxis xaxis2 = new CategoryAxis() { Position = AxisPosition.Top, MajorGridlineStyle = LineStyle.None, MinorGridlineStyle = LineStyle.None };
                Model.Axes.Add(xaxis2);

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

                Model.Series.Add(s1);
            }
            else
            {
                Model.Title = $"Categories per month";

                var categoriesForMonth =
                     App.Database.GetTransactionsAsync()
                     .Result
                     .Where(t => t.Date.Month == (int)Month && t.Date.Year == Year)
                     .GroupBy(t => t.TransactionType)
                     .Select(g => new { Type = g.Key, Amount = g.Sum(t => t.Amount) })
                     .ToList();

                foreach (var c in categoriesForMonth)
                {
                    xaxis.Labels.Add(c.Type.ToString());
                    s1.Items.Add(new ColumnItem(decimal.ToDouble(c.Amount)));
                }

                Model.Series.Add(s1);
            }
        }
        #endregion
    }
}
