using Training_Project.Model;

namespace Training_Project
{
    class Program
    {
        public static void runFinanceApp()
        {
            FinanceManager manager = new FinanceManager();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Personal Finance Manager ---");
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View All Transactions");
                Console.WriteLine("3. View Transactions by Category");
                Console.WriteLine("4. Add Category");
                Console.WriteLine("5. Modify Category");
                Console.WriteLine("6. Delete Category");
                Console.WriteLine("7. Modify Transaction");
                Console.WriteLine("8. Delete Transaction");
                Console.WriteLine("9. View Total Balance");
                Console.WriteLine("10. Sort Transactions");
                Console.WriteLine("11. Filter Transactions");
                Console.WriteLine("12. Exit");

                Console.Write("Choose an option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1": // Add Transaction
                        manager.AddTransaction();
                        break;

                    case "2": // View all Transactions
                        manager.ShowAllTransactions();
                        break;

                    case "3": // View Transactions by Category
                        manager.ShowTransactionsByCategory();
                        break;

                    case "4": // Add Category
                        manager.AddCategory();
                        break;

                    case "5": // Modify Category
                        manager.ModifyCategory();
                        break;

                    case "6": // Delete category
                        manager.DeleteCategory();
                        break;

                    case "7":  // Modify transaction
                        manager.ModifyTransaction();
                        break;

                    case "8":  // Delete transaction
                        manager.DeleteTransaction();
                        break;

                    case "9": // View total balance
                        Console.WriteLine($"Total Balance: {manager.GetTotalBalance():C}");
                        break;

                    case "10":
                        manager.SortTransactions();
                        break;

                    case "11":

                        break;
                    case "12":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

        }

        static void Main(string[] args)
        {
            runFinanceApp();
        }
    }
}