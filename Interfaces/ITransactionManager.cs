using Training_Project.Model;

namespace Training_Project.Interfaces
{
    public interface ITransactionManager
    {
        void Add();
        void Remove();
        void Modify();
        List<Transaction> GetAll();
        void ShowAll(bool showId);
        void Search();
        void Sort();
        void Filter();
    }
}
