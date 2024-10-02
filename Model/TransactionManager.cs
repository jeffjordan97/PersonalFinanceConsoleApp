
using System.Text.Json;

namespace Training_Project.Model
{
    internal class TransactionManager
    {
        private const string TransactionsFileName = "transactions.json";
        private List<Transaction> transactions;
        private int nextTransactionId; // Tracks the next unique ID
        private CategoryManager categoryManager;

        public TransactionManager()
        {
            transactions = new List<Transaction>(); // Initialize the list
            LoadTransactions();
            nextTransactionId = transactions.Any() ? transactions.Max(t => t.Id) + 1 : 1; // Initialize ID tracking
            categoryManager = new CategoryManager(this);
        }

        public void AddTransaction()
        {
            Console.WriteLine("--- NEW TRANSACTION ---");
            Console.Write("Enter description: ");
            string? description = Console.ReadLine()?.Trim();
            while (string.IsNullOrWhiteSpace(description))
            {
                Console.Write("Invalid value. Please enter a valid Description: ");
                description = Console.ReadLine();
            }

            decimal amount;
            Console.Write("Enter amount (£): ");
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
            {
                Console.Write("Invalid value. Please enter a valid positive amount: ");
            }

            Console.Write("Is this Income or Expense? (I/E): ");
            string? typeInput = Console.ReadLine()?.ToUpper();
            while (typeInput != "I" && typeInput != "E")
            {
                Console.WriteLine("Invalid input. Please enter 'I' for Income or 'E' for Expense: ");
                typeInput = Console.ReadLine()?.ToUpper();
            }
            TransactionType type = typeInput == "I" ? TransactionType.Income : TransactionType.Expense;

            Console.WriteLine("--- CATEGORIES ---");
            categoryManager.ShowAllCategories();
            List<string> categories = categoryManager.GetCategories();
            string? category;
            do
            {
                Console.Write("Enter a category: ");
                category = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(category) || !categories.Contains(category))
                {
                    Console.WriteLine("Invalid category. Please try again.");
                }
            }
            while (string.IsNullOrWhiteSpace(category) || !categories.Contains(category));

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

            Console.WriteLine("\n--- TRANSACTIONS ---");
            ShowAllTransactionsWithId();

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

            Console.WriteLine("\n--- TRANSACTIONS ---");
            ShowAllTransactions();

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

        // Get all transactions that belong to a specific category
        public List<Transaction> GetTransactionsByCategory(string category)
        {
            return transactions.Where(t => t.Category == category).ToList();
        }

        // Modify the Category name for transactions
        public void ModifyCategoryForTransactions(string category, string newCategory)
        {
            transactions.ForEach(transaction =>
            {
                if (transaction.Category.Equals(category))
                {
                    transaction.Category = newCategory;
                }
            });

        }

        // Get all transactions
        public List<Transaction> GetAllTransactions()
        {
            return transactions;
        }

        // Show all transactions
        public void ShowAllTransactions()
        {
            Console.WriteLine("\n--- TRANSACTIONS ---");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
            Console.WriteLine("------");
        }

        // Display transactions with their corresponding ID
        public void ShowAllTransactionsWithId()
        {
            Console.WriteLine("\n--- TRANSACTIONS ---");
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"ID: {transaction.Id} | {transaction.ToString()}");
            }
            Console.WriteLine("------");
        }

        // Sort Transactions by date, amount, category
        public void SortTransactions()
        {
            List<string> sortByCategories = new List<string> { "Amount", "Category", "Date", "Type" };
            Console.WriteLine("--- Sort Options ---");
            foreach (var sort in sortByCategories)
            {
                Console.WriteLine(sort);
            }
            Console.WriteLine("------");
            Console.Write("Sort by: ");
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
