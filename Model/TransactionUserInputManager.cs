namespace Training_Project.Model
{
    internal class TransactionUserInputManager
    {
        public TransactionUserInputManager() { }

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

        // User input for a valid Date
        public DateTime GetValidDate(string prompt)
        {
            DateTime validDate;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                // Try to parse the input using the specified date format
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out validDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter the date in yyyy-MM-dd format.");
                }
            }
            return validDate;
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


    }
}
