namespace ExpenseTracker.Models
{
    public class CategoryInfo
    {
        #region Properties
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }

        public string TextColor {
            get {
                switch (TransactionType)
                {
                    case TransactionType.INCOME: return "Green";
                    default: return "IndianRed";
                }
            }
        }

        public string ImageUrl { get { return $"{TransactionType}.png"; } }
        #endregion

        #region Constructors
        public CategoryInfo(TransactionType transactionType, decimal amount)
        {
            TransactionType = transactionType;
            Amount = amount;
        }
        #endregion
    }
}
