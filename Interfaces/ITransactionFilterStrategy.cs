using Training_Project.Model;

namespace Training_Project.Interfaces
{
    public interface ITransactionFilterStrategy
    {
        List<Transaction> Filter(List<Transaction> transactions);
    }
}
