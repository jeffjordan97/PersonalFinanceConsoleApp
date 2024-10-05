using Training_Project.Interfaces;
using Training_Project.Model;

namespace Training_Project.Managers
{
    internal class TransactionManager : ITransactionManager
    {
        private const string TransactionsFileName = "transactions.json";
        private List<Transaction> transactions;
        private int nextTransactionId; // Tracks the next unique ID
        private ICategoryManager<string> categoryManager;
        private ITransactionUserInputManager transactionUserInput;
        private ITransactionFileManager fileManager;

        public TransactionManager(ITransactionFileManager fileManager)
        {
            this.fileManager = fileManager;

            transactions = fileManager.LoadTransactions(TransactionsFileName) ?? new List<Transaction>();
            nextTransactionId = transactions.Any() ? transactions.Max(t => t.Id) + 1 : 1; // Initialize ID tracking
        }

        public void SetCategoryManager(ICategoryManager<string> categoryManager)
        {
            this.categoryManager = categoryManager;
        }

        public void SetTransactionUserInputManager(ITransactionUserInputManager transactionUserInputManager)
        {
            this.transactionUserInput = transactionUserInputManager;
        }

        //Add a new transaction
        public void Add()
        {
            Console.WriteLine("--- NEW TRANSACTION ---");
            string description = transactionUserInput.GetDescriptionInput();
            decimal amount = transactionUserInput.GetAmountInput();
            TransactionType type = transactionUserInput.GetTransactionType("Is this an Income or Expense? (Enter I/E): ");

            categoryManager.ShowAll();
            string category = transactionUserInput.GetCategoryInput();
            categoryManager.Add(category);

            var transaction = new Transaction(description, amount, type, category, DateTime.Now)
            {
                Id = nextTransactionId++ // Automatically assigns a unique Id
            };

            transactions.Add(transaction);
            fileManager.SaveTransactions(TransactionsFileName, transactions);
            Console.WriteLine("Transaction added with ID: " + transaction.Id);
        }

        // Remove transaction by its Id
        public void Remove()
        {
            int id;
            ShowAll(true);

            Console.Write("Enter transaction ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Invalid value.");
                return;
            }

            var transaction = transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                transactions.Remove(transaction);
                Save();
                Console.WriteLine("Transaction deleted.");
            }
            else
            {
                Console.WriteLine("Transaction not found.");
            }
        }

        // Modify a specific transaction by its Id
        public void Modify()
        {
            ShowAll(true);
            int id = transactionUserInput.GetTransactionId();

            var transaction = transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                transaction = transactionUserInput.UpdateTransactionFields(transaction);

                // New Category entered during modification
                if (!categoryManager.GetAll().Contains(transaction.Category))
                {
                    categoryManager.Add(transaction.Category);
                }
                Save();
                Console.WriteLine($"Transaction with ID {id} modified.");
            }
            else
            {
                Console.WriteLine($"Transaction with ID {id} not found.");
            }
        }

        // Get all transactions
        public List<Transaction> GetAll()
        {
            return transactions;
        }

        // Show all transactions
        public void ShowAll(bool showId)
        {
            Console.WriteLine("\n--- TRANSACTIONS ---");
            if (showId)
            {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"ID: {transaction.Id} | {transaction.ToString()}");

                }
            }
            else
            {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine(transaction.ToString());
                }
            }
            Console.WriteLine("------");
        }

        // Search for a transaction
        public void Search()
        {
            Console.WriteLine("Enter search criteria (Press Enter to skip any field):");


        }

        // Sort Transactions by date, amount, category
        public void Sort()
        {
            Console.WriteLine("--- Sort Options: ---");
            Console.WriteLine("Amount");
            Console.WriteLine("Category");
            Console.WriteLine("Date");
            Console.WriteLine("Type");
            Console.WriteLine("------");
            Console.Write("Enter a type to sort by: ");
            string? sortBy = Console.ReadLine();

            List<Transaction> sortedTransactions = new List<Transaction>();
            switch (sortBy.ToLower())
            {
                case "amount":
                    sortedTransactions = transactions.OrderBy(t => t.Amount).ToList();
                    break;
                case "category":
                    sortedTransactions = transactions.OrderBy(t => t.Category).ToList();
                    break;
                case "date":
                    sortedTransactions = transactions.OrderBy(t => t.Date).ToList();
                    break;
                case "type":
                    sortedTransactions = transactions.OrderBy(t => t.Type).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid sorting option.");
                    return;
            }

            // Display the sorted transactions
            Console.WriteLine("\n--- Sorted Transactions ---");
            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        public List<Transaction> FilterByCategory()
        {
            List<string> categories = categoryManager.GetAll();
            if (categories != null)
            {
                categoryManager.ShowAll();
            }
            else
            {
                Console.WriteLine("No categories. Please add a transaction from the main menu.");
                return [];  // Return empty list
            }

            string category;
            do
            {
                category = transactionUserInput.GetCategoryInput();
                if (categories.Contains(category))
                {
                    break;
                }
                Console.WriteLine("Invalid Category value. Please try again.");
            }
            while (!categories.Contains(category));
            return transactions.Where(t => t.Category.ToLower() == category.ToLower()).ToList();
        }

        // Filter transactions
        public void Filter()
        {
            List<Transaction> filteredTransactions = new List<Transaction>();
            Console.WriteLine("--- Filter Options: ---");
            Console.WriteLine("Type");
            Console.WriteLine("Category");
            Console.WriteLine("Date");
            Console.WriteLine("------");
            Console.Write("Enter a type to filter by: ");
            string? filterBy = Console.ReadLine();

            switch (filterBy.ToLower())
            {
                case "type":
                    TransactionType type = transactionUserInput.GetTransactionType("Filter by Income Or Expense? (Enter I/E): ");
                    filteredTransactions = transactions.Where(t => t.Type == type).ToList();

                    break;
                case "category":
                    filteredTransactions = FilterByCategory();
                    break;
                case "date":
                    DateTime startDate;
                    DateTime endDate;
                    do
                    {
                        startDate = transactionUserInput.GetValidDate("Enter start date (dd-MM-yyyy): ");
                        endDate = transactionUserInput.GetValidDate("Enter end date (dd-MM-yyyy): ");
                        // Check if the end date is not earlier than the start date
                        if (endDate < startDate)
                        {
                            Console.WriteLine("End date cannot be earlier than start date. Please enter the dates again.");
                        }
                    }
                    while (endDate < startDate);
                    filteredTransactions = transactions.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid filter option.");
                    return;
            }

            if (filteredTransactions.Count == 0)
            {
                Console.WriteLine("No Transactions.");
                return;
            }
            // Display the filtered transactions
            Console.WriteLine("\n--- Filtered Transactions ---");
            foreach (var transaction in filteredTransactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        // Method to save transactions to a JSON file
        private void Save()
        {
            fileManager.SaveTransactions(TransactionsFileName, transactions);
        }

        // Method to load transactions from a JSON file
        private void Load()
        {
            transactions = fileManager.LoadTransactions(TransactionsFileName);
        }

        // Get the total balance of transactions (Income - Expenses)
        public decimal GetTotalBalance()
        {
            return transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        }

    }

}
