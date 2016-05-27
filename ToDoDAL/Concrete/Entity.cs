using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ToDoDAL.Abstract;

namespace ToDoDAL.Concrete
{
    public abstract class Entity : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
