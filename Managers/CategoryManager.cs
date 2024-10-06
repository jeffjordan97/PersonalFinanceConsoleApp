using Training_Project.Interfaces;
using Training_Project.Model;

namespace Training_Project.Managers
{
    public class CategoryManager : ICategoryManager<string>
    {
        private List<string> categories;

        public CategoryManager(List<Transaction> transactions)
        {
            categories = transactions.Select(t => t.Category).Distinct().ToList();
        }

        // Return a list of all categories
        public List<string> GetAll()
        {
            return categories;
        }

        public void Add(string category)
        {
            if (!categories.Contains(category.ToLower(), StringComparer.OrdinalIgnoreCase))
            {
                categories.Add(category);
            }
        }

        public void Remove(string category)
        {
            categories.Remove(category);
        }

        public void ShowAll()
        {
            Console.WriteLine($"--- CATEGORIES ---");
            foreach (var category in categories)
            {
                Console.WriteLine($"{category}");
            }
            Console.WriteLine("------");
        }
    }
}
