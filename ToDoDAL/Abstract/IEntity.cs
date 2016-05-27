using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoDAL.Abstract
{
    public interface IEntity 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }    
    }
}
