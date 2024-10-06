using Training_Project.Interfaces;
using Training_Project.Model;

namespace Training_Project.Managers
{
    public class TransactionFilterManager
    {
        private ITransactionUserInputManager _userInputManager;
        private ICategoryManager<string> _categoryManager;

        public TransactionFilterManager(TransactionUserInputManager userInputManager, CategoryManager categoryManager)
        {
            _userInputManager = userInputManager;
            _categoryManager = categoryManager;
        }

        public TransactionFilterManager(ITransactionUserInputManager userInputManager, ICategoryManager<string> categoryManager)
        {
            this._userInputManager = userInputManager;
            this._categoryManager = categoryManager;
        }

        public ITransactionFilterStrategy? GetFilterStrategy(string filterBy)
        {
            switch (filterBy.ToLower())
            {
                case "type":
                    return new FilterByType(_userInputManager);
                case "category":
                    return new FilterByCategory(_categoryManager, _userInputManager);
                case "date":
                    return new FilterByDate(_userInputManager);
                default:
                    return null;
            }
        }
    }

    public class FilterByType : ITransactionFilterStrategy
    {
        private readonly ITransactionUserInputManager _userInputManager;

        public FilterByType(ITransactionUserInputManager userInputManager)
        {
            _userInputManager = userInputManager;
        }

        public List<Transaction> Filter(List<Transaction> transactions)
        {
            TransactionType type = _userInputManager.GetTransactionType("Filter by Income Or Expense? (Enter I/E): ");
            return transactions.Where(t => t.Type == type).ToList();
        }
    }

    public class FilterByCategory : ITransactionFilterStrategy
    {
        private readonly ICategoryManager<string> _categoryManager;
        private readonly ITransactionUserInputManager _userInputManager;

        public FilterByCategory(ICategoryManager<string> categoryManager, ITransactionUserInputManager userInputManager)
        {
            _categoryManager = categoryManager;
            _userInputManager = userInputManager;
        }

        public List<Transaction> Filter(List<Transaction> transactions)
        {
            List<string> categories = _categoryManager.GetAll();
            if (categories != null)
            {
                _categoryManager.ShowAll();
            }
            else
            {
                Console.WriteLine("No categories. Please add a transaction from the main menu.");
                return [];  // Return empty list
            }

            string category;
            do
            {
                category = _userInputManager.GetCategoryInput();
                if (categories.Contains(category))
                {
                    break;
                }
                Console.WriteLine("Invalid Category value. Please try again.");
            }
            while (!categories.Contains(category));
            return transactions.Where(t => t.Category.ToLower() == category.ToLower()).ToList();
        }
    }

    public class FilterByDate : ITransactionFilterStrategy
    {
        private readonly ITransactionUserInputManager _userInputManager;

        public FilterByDate(ITransactionUserInputManager userInputManager)
        {
            _userInputManager = userInputManager;
        }

        public List<Transaction> Filter(List<Transaction> transactions)
        {
            DateTime startDate;
            DateTime endDate;
            do
            {
                startDate = _userInputManager.GetValidDate("Enter start date (dd-MM-yyyy): ");
                endDate = _userInputManager.GetValidDate("Enter end date (dd-MM-yyyy): ");
                if (endDate < startDate)
                {
                    Console.WriteLine("End date cannot be earlier than start date. Please enter the dates again.");
                }
            }
            while (endDate < startDate);

            return transactions.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();
        }
    }
}
