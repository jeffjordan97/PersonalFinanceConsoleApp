
using System.Text.Json;

namespace Training_Project.Model
{
    internal class TransactionManager
    {
        private const string TransactionsFileName = "transactions.json";
        private List<Transaction> transactions;
        private int nextTransactionId; // Tracks the next unique ID
        private CategoryManager categoryManager;
        private TransactionUserInputManager transactionUserInput;

        public TransactionManager()
        {
            transactions = new List<Transaction>(); // Initialize the list
            LoadTransactions();
            nextTransactionId = transactions.Any() ? transactions.Max(t => t.Id) + 1 : 1; // Initialize ID tracking
            categoryManager = new CategoryManager(transactions);
            transactionUserInput = new TransactionUserInputManager();
        }

        //Add a new transaction
        public void AddTransaction()
        {
            Console.WriteLine("--- NEW TRANSACTION ---");
            // Description
            string description = transactionUserInput.GetDescriptionInput();
            // Amount
            decimal amount = transactionUserInput.GetAmountInput();
            // Type (I/E)
            TransactionType type = transactionUserInput.GetTransactionType("Is this an Income or Expense? (Enter I/E): ");
            // Category
            categoryManager.ShowAllCategories();
            List<string> categories = categoryManager.GetCategories();
            string category = transactionUserInput.GetCategoryInput();
            categoryManager.AddCategory(category);
            // Date
            var date = DateTime.Now;

            var transaction = new Transaction(description, amount, type, category, date)
            {
                Id = nextTransactionId++ // Automatically assigns a unique Id
            };
            transactions.Add(transaction);
            SaveTransactions();
            Console.WriteLine("Transaction added with ID: " + transaction.Id);
        }

        // Modify a specific transaction by its Id
        public void ModifyTransaction()
        {
            int id;
            ShowAllTransactions(true);

            Console.Write("Enter transaction ID to modify:");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Invalid value.");
                return;
            }

            var transaction = transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                // Description (allow skipping by pressing enter)
                Console.Write("Enter description (or press Enter to skip): ");
                string? description = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(description))
                {
                    transaction.Description = description;
                }

                // Amount (allow skipping by pressing enter)
                Console.Write("Enter amount (or press Enter to skip): ");
                string? amountToParse = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(amountToParse) && decimal.TryParse(amountToParse, out decimal amount))
                {
                    transaction.Amount = amount;
                }
                else if (!string.IsNullOrWhiteSpace(amountToParse))
                {
                    Console.WriteLine("Invalid value for amount, keeping the current amount.");
                }

                // Income or Expense (allow skipping by pressing enter)
                Console.Write("Is this Income or Expense? (I/E) (or press Enter to skip): ");
                string? typeInput = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(typeInput))
                {
                    if (typeInput.Equals("I", StringComparison.CurrentCultureIgnoreCase))
                    {
                        transaction.Type = TransactionType.Income;
                    }
                    else if (typeInput.Equals("E", StringComparison.CurrentCultureIgnoreCase))
                    {
                        transaction.Type = TransactionType.Expense;
                    }
                    else
                    {
                        Console.WriteLine("Invalid type input, keeping the current type.");
                    }
                }

                // Category (allow skipping by pressing enter)
                Console.WriteLine("--- CATEGORIES ---");
                categoryManager.ShowAllCategories();
                List<string> categories = categoryManager.GetCategories();

                Console.Write("Enter a category (or press Enter to skip): ");
                string? category = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(category) && categories.Contains(category))
                {
                    transaction.Category = category;
                }
                else if (!string.IsNullOrWhiteSpace(category))
                {
                    Console.WriteLine("Invalid category, keeping the current category.");
                }

                // Update the transaction date to the current date
                transaction.Date = DateTime.Now;

                // Save changes
                SaveTransactions();
                Console.WriteLine("Transaction modified.");
            }
            else
            {
                Console.WriteLine($"Transaction with ID {id} not found.");
            }
        }

        // Remove transaction by its Id
        public void DeleteTransaction()
        {
            int id;
            ShowAllTransactions(true);

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
                SaveTransactions();
                Console.WriteLine("Transaction deleted.");
            }
            else
            {
                Console.WriteLine("Transaction not found.");
            }
        }

        // Search for a transaction
        public void SearchTransactions()
        {
            Console.WriteLine("Enter search criteria (Press Enter to skip any field):");


        }

        // Get all transactions
        public List<Transaction> GetAllTransactions()
        {
            return transactions;
        }

        // Show all transactions
        public void ShowAllTransactions(bool showId)
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

        // Sort Transactions by date, amount, category
        public void SortTransactions()
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
            List<string> categories = categoryManager.GetCategories();
            if (categories != null)
            {
                categoryManager.ShowAllCategories();
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
        public void FilterTransactions()
        {
            List<Transaction> filteredTransactions = new List<Transaction>();
            Console.WriteLine("--- Filter Options: ---");
            Console.WriteLine("Type");
            Console.WriteLine("Category");
            Console.WriteLine("Date Range");
            Console.WriteLine("------");
            Console.Write("Enter a type to filter by: ");
            string? filterBy = Console.ReadLine();

            switch (filterBy.ToLower())
            {
                case "type":
                    TransactionType type = transactionUserInput.GetTransactionType("Filter by Income Or Expense? (Enter I/E):");
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
                        startDate = transactionUserInput.GetValidDate("Enter start date (yyyy-MM-dd): ");
                        endDate = transactionUserInput.GetValidDate("Enter end date (yyyy-MM-dd): ");
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
        private void SaveTransactions()
        {
            var json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(TransactionsFileName, json);
        }

        // Method to load transactions from a JSON file
        private void LoadTransactions()
        {
            if (File.Exists(TransactionsFileName))
            {
                var json = File.ReadAllText(TransactionsFileName);
                transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? [];
            }
            else
            {
                transactions = [];
            }
        }

        // Get the total balance of transactions (Income - Expenses)
        public decimal GetTotalBalance()
        {
            return transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        }
    }

}
