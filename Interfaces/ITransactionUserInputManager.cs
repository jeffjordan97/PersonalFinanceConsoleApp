using Training_Project.Model;

namespace Training_Project.Interfaces
{
    public interface ITransactionUserInputManager
    {
        int GetTransactionId();
        string GetDescriptionInput();
        decimal GetAmountInput();
        TransactionType GetTransactionType(string prompt);
        string GetCategoryInput();
        Transaction UpdateTransactionFields(Transaction transaction);
        //string GetFilterOption();
        (DateTime startDate, DateTime endDate) GetDateRange();
        DateTime GetValidDate(string prompt);
        string? GetSortOption();
        string? GetFilterOption();
    }
}
