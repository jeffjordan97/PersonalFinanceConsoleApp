namespace Training_Project.Model
{
    public class Transaction(string description, decimal amount, TransactionType type, string category, DateTime date)
    {
        public int Id { get; set; }
        public string Description { get; set; } = description;
        public decimal Amount { get; set; } = amount;
        public TransactionType Type { get; set; } = type;
        public string Category { get; set; } = category;
        public DateTime Date { get; set; } = date;

        public override string ToString()
        {
            return $"{Date.ToShortDateString()}: {Description} - {Type} of {Amount:C} [{Category}]";
        }
    }
}
