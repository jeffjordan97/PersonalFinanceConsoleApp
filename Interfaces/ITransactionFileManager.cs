using Training_Project.Model;

namespace Training_Project.Interfaces
{
    internal interface ITransactionFileManager
    {
        List<Transaction> LoadTransactions(string filePath);
        void SaveTransactions(string filePath, List<Transaction> transactions);
    }
}
