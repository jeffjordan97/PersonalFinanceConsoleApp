using Training_Project.Interfaces;

namespace Training_Project.Managers
{
    public class MenuCommandsManager
    {

        public class AddTransactionCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public AddTransactionCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                _transactionManager.Add();
            }
        }

        public class ViewAllTransactionsCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public ViewAllTransactionsCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                bool showId = false;
                _transactionManager.ShowAll(showId);
            }
        }

        public class ModifyTransactionCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public ModifyTransactionCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                _transactionManager.Modify();
            }
        }

        public class DeleteTransactionCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public DeleteTransactionCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                _transactionManager.Remove();
            }
        }


        public class ViewTotalBalanceCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public ViewTotalBalanceCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                _transactionManager.GetTotalBalance();
            }
        }

        public class SortTransactionsCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public SortTransactionsCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                _transactionManager.Sort();
            }
        }

        public class FilterTransactionsCommand : IMenuCommand
        {
            private readonly TransactionManager _transactionManager;

            public FilterTransactionsCommand(TransactionManager transactionManager)
            {
                _transactionManager = transactionManager;
            }

            public void Execute()
            {
                _transactionManager.Filter();
            }
        }

        // Uses a delegate (a method reference) that modifies the running flag
        public class ExitCommand : IMenuCommand
        {
            private readonly Action _stopAction;

            public ExitCommand(Action stopAction)
            {
                _stopAction = stopAction;
            }

            public void Execute()
            {
                Console.WriteLine("Exiting...");
                _stopAction();  // This will stop the application
            }
        }
    }
}
