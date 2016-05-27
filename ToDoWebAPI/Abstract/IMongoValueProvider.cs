using System.Collections.Generic;


namespace ToDoWebAPI.Abstract
{
    public interface IMongoValueProvider<T>
    {
        IEnumerable<T> GetValues();
        T GetValue(string id);
        void CreateValue(T item);
        void UpdateValue(T item);
        void DeleteValue(string id);
    }
}
