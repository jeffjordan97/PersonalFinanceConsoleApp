using Training_Project.Model;

namespace Training_Project.Interfaces
{
    internal interface ITransactionSortStrategy
    {
        List<Transaction> Sort(List<Transaction> transactions);
    }
}
