namespace Training_Project.Model
{
    internal class FinanceManager
    {
        private TransactionManager transactionManager;
        private CategoryManager categoryManager;

        public FinanceManager()
        {
            transactionManager = new TransactionManager();
            categoryManager = new CategoryManager(transactionManager);
        }

        // TRANSACTION MANAGER METHODS
        // Add new transaction
        public void AddTransaction()
        {
            transactionManager.AddTransaction();
        }

        // Modify existing transaction by its Id
        public void ModifyTransaction()
        {
            transactionManager.ModifyTransaction();
        }

        // Remove transaction by its Id
        public void DeleteTransaction()
        {
            transactionManager.DeleteTransaction();
        }

        // Show all transactions
        public void ShowAllTransactions()
        {
            transactionManager.ShowAllTransactions();
        }

        // Get all transactions
        public List<Transaction> GetAllTransactions()
        {
            return transactionManager.GetAllTransactions();
        }

        // Get all transactions that belong to a specific category
        public void ShowTransactionsByCategory()
        {
            Console.WriteLine("\n--- CATEGORIES ---");
            categoryManager.ShowAllCategories();
            Console.Write("Choose a category to filter by: ");
            string? category = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(category) && categoryManager.GetCategories().Contains(category))
            {
                List<Transaction> transactions = transactionManager.GetTransactionsByCategory(category);
                foreach (Transaction transaction in transactions)
                {
                    Console.WriteLine(transaction.ToString());
                }
            }
            else
            {
                Console.WriteLine("Category not found.");
            }
        }

        // Get the total balance of transactions (Income -  Expenses)
        public decimal GetTotalBalance()
        {
            return transactionManager.GetTotalBalance();
        }

        // Sort transactions
        public void SortTransactions()
        {
            transactionManager.SortTransactions();
        }

        // CATEGORY MANAGER METHODS
        // Add a new category
        public void AddCategory()
        {
            categoryManager.AddCategory();
        }

        // Modify an existing category, replacing the old name with the new name
        public void ModifyCategory()
        {
            categoryManager.ModifyCategory();
        }

        // Remove a category by name
        public void DeleteCategory()
        {
            categoryManager.DeleteCategory();
        }

        // Get all categories
        public List<string> GetCategories()
        {
            return categoryManager.GetCategories();
        }

        // Display all categories
        public void ShowAllCategories()
        {
            categoryManager.ShowAllCategories();
        }
    }
}
