using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoDomainModels.Model.Mongo
{
    public abstract class Entity : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
