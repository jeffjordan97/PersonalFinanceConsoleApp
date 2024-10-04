namespace Training_Project.Model
{
    internal class CategoryManager
    {
        private const string TransactionsFileName = "transactions.json";
        private List<Transaction> transactions;
        private List<string> categories;

        public CategoryManager(List<Transaction> transactions)
        {
            categories = new List<string>(); // Initialize the list
            this.transactions = transactions;
            LoadCategories(); // Load categories from the transactions on initialization
        }

        // Return a list of all categories
        public List<string> GetCategories()
        {
            return categories;
        }

        public void AddCategory(string category)
        {
            if (!categories.Contains(category.ToLower()) || !categories.Contains(category.ToUpper()))
            {
                categories.Add(category);
            }
        }

        public void RemoveCategory(string category)
        {
            categories.Remove(category);
        }

        // Show all categories
        public void ShowAllCategories()
        {
            Console.WriteLine("--- CATEGORIES ---");
            foreach (var category in categories)
            {
                Console.WriteLine($"{category}");
            }
            Console.WriteLine("------");
        }

        // Method to load categories from transactions
        private void LoadCategories()
        {
            if (File.Exists(TransactionsFileName))
            {
                // Get distinct categories from transactions
                categories = transactions
                    .Select(t => t.Category)     // Select the Category field
                    .Distinct()                  // Get distinct categories
                    .ToList();                   // Convert to List
            }
        }
    }
}
