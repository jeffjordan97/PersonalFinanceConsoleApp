using System.Text.Json;
using Training_Project.Interfaces;
using Training_Project.Model;

namespace Training_Project.Utilities
{
    internal class JsonTransactionFileManager : ITransactionFileManager
    {
        public List<Transaction> LoadTransactions(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            }
            return new List<Transaction>();
        }

        public void SaveTransactions(string filePath, List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(filePath, json);
        }
    }
}
