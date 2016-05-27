using MongoDB.Bson.Serialization.Attributes;

namespace ToDoDAL.Abstract
{
    public interface IEntity 
    {
        [BsonId]
        string Id { get; set; }    
    }
}
