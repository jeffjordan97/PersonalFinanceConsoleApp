namespace Training_Project.Interfaces
{
    public interface ICategoryManager<T>
    {
        void Add(T item);
        void Remove(T item);
        List<T> GetAll();
        void ShowAll();
    }
}
