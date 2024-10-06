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
        private ITransactionUserInputManager userInputManager;
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

        public void SetuserInputManagerManager(ITransactionUserInputManager userInputManagerManager)
        {
            this.userInputManager = userInputManagerManager;
        }

        //Add a new transaction
        public void Add()
        {
            Console.WriteLine("--- NEW TRANSACTION ---");
            string description = userInputManager.GetDescriptionInput();
            decimal amount = userInputManager.GetAmountInput();
            TransactionType type = userInputManager.GetTransactionType("Is this an Income or Expense? (Enter I/E): ");

            categoryManager.ShowAll();
            string category = userInputManager.GetCategoryInput();
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
            int id = userInputManager.GetTransactionId();

            var transaction = transactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                transaction = userInputManager.UpdateTransactionFields(transaction);

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

        // Apply strategy to sort or filter
        private void ApplyStrategy<TStrategy>(TStrategy? strategy, string prompt) where TStrategy : class
        {
            if (strategy == null)
            {
                Console.WriteLine("Invalid option.");
                return;
            }

            List<Transaction> resultTransactions = (strategy as ITransactionSortStrategy)?.Sort(transactions)
                                                   ?? (strategy as ITransactionFilterStrategy)?.Filter(transactions)
                                                   ?? new List<Transaction>();

            // Display the result transactions
            DisplayTransactions(resultTransactions, prompt);
        }

        // General method to display a list of transactions
        private void DisplayTransactions(List<Transaction> transactions, string prompt)
        {
            if (transactions.Count == 0)
            {
                Console.WriteLine("No Transactions.");
                return;
            }

            Console.WriteLine($"\n--- {prompt} ---");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.ToString());
            }
        }

        // Sort Transactions by date, amount, category
        public void Sort()
        {
            string? sortBy = userInputManager.GetSortOption();
            TransactionSortManager sortManager = new TransactionSortManager();
            ITransactionSortStrategy? sortingStrategy = sortManager.GetSortingStrategy(sortBy);

            ApplyStrategy(sortingStrategy, "Sorted Transactions");
        }

        // Filter transactions
        public void Filter()
        {
            string? filter = userInputManager.GetFilterOption();
            TransactionFilterManager filterManager = new TransactionFilterManager(userInputManager, categoryManager);
            ITransactionFilterStrategy? filterStrategy = filterManager.GetFilterStrategy(filter);

            ApplyStrategy(filterStrategy, "Filtered Transactions");
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
