using System.Collections.Generic;

namespace ToDoProviders
{
    public interface IMongoValueProvider<T>
    {
        IEnumerable<T> GetValues();

        T GetValue(string id);

        T CreateValue(T item);

        void UpdateValue(T item);

        void DeleteValue(string id);
    }
}
