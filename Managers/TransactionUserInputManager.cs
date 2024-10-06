using Training_Project.Interfaces;
using Training_Project.Model;

namespace Training_Project.Managers
{
    internal class TransactionUserInputManager : ITransactionUserInputManager
    {
        ICategoryManager<string> categoryManager;

        public TransactionUserInputManager(ICategoryManager<string> categoryManager)
        {
            this.categoryManager = categoryManager;
        }

        public int GetTransactionId()
        {
            Console.Write("Enter transaction ID to modify:");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Invalid value.");
            }
            return id;
        }

        // User input for Description
        public string GetDescriptionInput()
        {
            Console.Write("Enter description: ");
            string? description = Console.ReadLine()?.Trim();
            while (string.IsNullOrWhiteSpace(description))
            {
                Console.Write("Invalid value. Please enter a valid Description: ");
                description = Console.ReadLine();
            }
            return description;
        }

        // User input for amount
        public decimal GetAmountInput()
        {
            decimal amount;
            Console.Write("Enter amount (£): ");
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
            {
                Console.Write("Invalid value. Please enter a valid positive amount: ");
            }
            return amount;
        }

        // User input for Transaction Type
        public TransactionType GetTransactionType(string prompt)
        {
            Console.Write(prompt);
            string? typeInput = Console.ReadLine()?.ToUpper();
            while (typeInput != "I" && typeInput != "E")
            {
                Console.WriteLine("Invalid input. Please enter 'I' for Income or 'E' for Expense: ");
                typeInput = Console.ReadLine()?.ToUpper();
            }
            return typeInput == "I" ? TransactionType.Income : TransactionType.Expense;
        }

        // User input for category
        public string GetCategoryInput()
        {
            string? category;
            do
            {
                Console.Write("Enter a category: ");
                category = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(category))
                {
                    Console.WriteLine("Invalid value. Please try again.");
                }
            }
            while (string.IsNullOrWhiteSpace(category));
            return category;
        }

        public Transaction UpdateTransactionFields(Transaction transaction)
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
            this.categoryManager.ShowAll();
            List<string> categories = categoryManager.GetAll();

            Console.Write("Enter a category (or press Enter to skip): ");
            string? category = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(category))
            {
                transaction.Category = category;
            }

            // Update the transaction date to the current date
            transaction.Date = DateTime.Now;

            return transaction;
        }

        public (DateTime startDate, DateTime endDate) GetDateRange()
        {
            DateTime startDate = GetValidDate("Enter Start Date (yyyy-MM-dd): ");
            DateTime endDate = GetValidDate("Enter End Date (yyyy-MM-dd): ");
            return (startDate, endDate);
        }

        // User input for a valid Date
        public DateTime GetValidDate(string prompt)
        {
            DateTime validDate;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                // Try to parse the input using the specified date format
                if (DateTime.TryParseExact(input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out validDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter the date in dd-MM-yyyy format.");
                }
            }
            return validDate;
        }

    }
}
