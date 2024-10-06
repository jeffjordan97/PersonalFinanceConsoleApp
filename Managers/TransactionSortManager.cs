using Training_Project.Interfaces;
using Training_Project.Model;

namespace Training_Project.Managers
{
    internal class TransactionSortManager
    {
        public ITransactionSortStrategy? GetSortingStrategy(string sortBy)
        {
            switch (sortBy.ToLower())
            {
                case "amount":
                    return new SortByAmount();
                case "category":
                    return new SortByCategory();
                case "date":
                    return new SortByDate();
                case "type":
                    return new SortByType();
                default:
                    return null;
            }
        }
    }

    public class SortByAmount : ITransactionSortStrategy
    {
        public List<Transaction> Sort(List<Transaction> transactions)
        {
            return transactions.OrderBy(t => t.Amount).ToList();
        }
    }

    public class SortByCategory : ITransactionSortStrategy
    {
        public List<Transaction> Sort(List<Transaction> transactions)
        {
            return transactions.OrderBy(t => t.Category).ToList();
        }
    }

    public class SortByDate : ITransactionSortStrategy
    {
        public List<Transaction> Sort(List<Transaction> transactions)
        {
            return transactions.OrderBy(t => t.Date).ToList();
        }
    }

    public class SortByType : ITransactionSortStrategy
    {
        public List<Transaction> Sort(List<Transaction> transactions)
        {
            return transactions.OrderBy(t => t.Type).ToList();
        }
    }

}
