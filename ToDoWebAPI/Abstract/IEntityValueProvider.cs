using System.Collections.Generic;

namespace ToDoWebAPI.Abstract
{
    public interface IEntityValueProvider<T> where T:class
    {
        IEnumerable<T> GetValues();
        T GetValue(int id);
        void CreateValue(T item);
        void UpdateValue(T item);
        void DeleteValue(int id);
    }
}
