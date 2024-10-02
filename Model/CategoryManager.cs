using System.Text.Json;

namespace Training_Project.Model
{
    internal class CategoryManager
    {
        private const string CategoriesFileName = "categories.json";
        private List<string> categories;
        private readonly List<string> defaultCategories = ["Food", "Rent", "Utilities", "Entertainment"]; // Default categories
        private TransactionManager transactionManager;

        public CategoryManager(TransactionManager transactionManager)
        {
            categories = new List<string>(); // Initialize the list
            LoadCategories(); // Load categories from the file on initialization
            this.transactionManager = transactionManager;
        }

        // Add a new category to the list
        public void AddCategory()
        {
            Console.WriteLine("\n--- CATEGORIES ---");
            ShowAllCategories();

            Console.Write("Enter a category: ");
            string? category = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Invalid category input.");
            }
            else if (categories.Contains(category))
            {
                Console.WriteLine("Category already exists.");
            }
            else
            {
                categories.Add(category);
                SaveCategories();
                Console.WriteLine("Category added.");
            }
        }

        // Modify a category, removing the old category and adding the new category
        public void ModifyCategory()
        {
            Console.WriteLine("\n--- CATEGORIES ---");
            ShowAllCategories();

            Console.Write("Enter the category name to modify: ");
            string? category = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Invalid category input.");
            }
            else if (!categories.Contains(category))
            {
                categories.Add(category);
                SaveCategories();
                Console.WriteLine("Category does not exists. Added as new category.");
            }
            else
            {
                Console.Write("Enter the new category name: ");
                string? newCategory = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(newCategory))
                {
                    Console.WriteLine("Invalid category input.");
                }
                else
                {
                    categories.Remove(category);
                    categories.Add(newCategory);
                    transactionManager.ModifyCategoryForTransactions(category, newCategory);
                    SaveCategories();
                    Console.WriteLine("Category modified.");
                }
            }
        }

        // Remove a category by its name
        public void DeleteCategory()
        {
            Console.WriteLine("\n--- CATEGORIES ---");
            ShowAllCategories();

            Console.Write("Enter the category name to delete: ");
            string? category = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(category) && categories.Contains(category))
            {
                categories.Remove(category);
                SaveCategories();
                Console.WriteLine("Category deleted.");
            }
            else
            {
                Console.WriteLine("Category not found.");
            }
        }

        // Return a list of all categories
        public List<string> GetCategories()
        {
            return categories;
        }

        // Show all categories
        public void ShowAllCategories()
        {
            foreach (var category in categories)
            {
                Console.WriteLine($"{category}");
            }
        }

        // Method to save categories to a JSON file
        private void SaveCategories()
        {
            var json = JsonSerializer.Serialize(categories);
            File.WriteAllText(CategoriesFileName, json);
        }

        // Method to load categories from a JSON file
        private void LoadCategories()
        {
            if (File.Exists(CategoriesFileName))
            {
                var json = File.ReadAllText(CategoriesFileName);
                categories = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            else
            {
                categories = defaultCategories;
            }
        }
    }
}
