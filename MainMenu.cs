using Training_Project.Interfaces;
using Training_Project.Managers;
using Training_Project.Utilities;
using static Training_Project.Managers.MenuCommandsManager;

namespace Training_Project
{
    public class MainMenu
    {
        private JsonTransactionFileManager fileManager;
        private TransactionManager transactionManager;
        private CategoryManager categoryManager;
        private TransactionUserInputManager userInputManager;

        public MainMenu()
        {
            fileManager = new JsonTransactionFileManager();
            transactionManager = new TransactionManager(fileManager);
            categoryManager = new CategoryManager(transactionManager.GetAll());
            userInputManager = new TransactionUserInputManager(categoryManager);

            transactionManager.SetCategoryManager(categoryManager);
            transactionManager.SetTransactionUserInputManager(userInputManager);
        }

        public void Start()
        {
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
                Console.WriteLine("------");

                // Initialize commands
                var commands = new Dictionary<string, IMenuCommand>
                {
                    { "1", new AddTransactionCommand(transactionManager) },
                    { "2", new ViewAllTransactionsCommand(transactionManager) },
                    { "3", new ModifyTransactionCommand(transactionManager) },
                    { "4", new DeleteTransactionCommand(transactionManager) },
                    { "5", new ViewTotalBalanceCommand(transactionManager) },
                    { "6", new SortTransactionsCommand(transactionManager) },
                    { "7", new FilterTransactionsCommand(transactionManager) },
                    { "8", new ExitCommand(() => { running = false; }) }  // Exit Command with delegate
                };

                // Input handling loop
                while (running)
                {
                    Console.Write("Choose an option: ");
                    string? input = Console.ReadLine();

                    if (commands.TryGetValue(input, out var command))
                    {
                        command.Execute();
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                    }
                }
            }
        }
    }
}
