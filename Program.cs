using Training_Project.Managers;
using Training_Project.Utilities;

namespace Training_Project
{
    class Program
    {
        public static void runFinanceApp()
        {
            JsonTransactionFileManager fileManager = new JsonTransactionFileManager();
            TransactionManager manager = new TransactionManager(fileManager);
            CategoryManager categoryManager = new CategoryManager(manager.GetAll());
            TransactionUserInputManager userInputManager = new TransactionUserInputManager();

            manager.SetCategoryManager(categoryManager);
            userInputManager.SetCategoryManager(categoryManager);
            manager.SetTransactionUserInputManager(userInputManager);


            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Personal Finance Manager ---");
                Console.WriteLine("1. Add Transaction");
                Console.WriteLine("2. View All Transactions");
                Console.WriteLine("3. Modify Transaction");
                Console.WriteLine("4. Delete Transaction");
                Console.WriteLine("5. View Total Balance");
                Console.WriteLine("6. Sort Transactions");
                Console.WriteLine("7. Filter Transactions");
                Console.WriteLine("8. Exit");

                Console.Write("Choose an option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1": // Add Transaction
                        manager.Add();
                        break;

                    case "2": // View all
                        bool showId = false;
                        manager.ShowAll(showId);
                        break;

                    case "3":  // Modify transaction
                        manager.Modify();
                        break;

                    case "4":  // Delete transaction
                        manager.Remove();
                        break;

                    case "5": // View total balance
                        Console.WriteLine($"Total Balance: {manager.GetTotalBalance():C}");
                        break;

                    case "6": // Sort Transactions
                        manager.Sort();
                        break;

                    case "7": // Filter Transactions
                        manager.Filter();
                        break;

                    case "8":
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