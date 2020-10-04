using ExpenseTracker.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class Database
    {
        #region Private fields
        private readonly SQLiteAsyncConnection _database;
        #endregion

        #region Constructors
        public Database()
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpenseTracker.db3");
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<Transaction>().Wait();
        }
        #endregion

        #region Methods
        public Task<List<Transaction>> GetTransactionsAsync()
        {
            return _database.Table<Transaction>().ToListAsync();
        }

        public Task<Transaction> GetTransactionAsync(int id)
        {
            return _database.Table<Transaction>()
                            .Where(t => t.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveTransactionAsync(Transaction transaction)
        {
            if (transaction.ID != 0)
            {
                return _database.UpdateAsync(transaction);
            }
            else
            {
                return _database.InsertAsync(transaction);
            }
        }

        public Task<int> DeleteTransactionAsync(Transaction transaction)
        {
            return _database.DeleteAsync(transaction);
        }
        #endregion
    }
}
