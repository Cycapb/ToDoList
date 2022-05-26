using MongoDB.Bson.Serialization.Attributes;

namespace ToDoDomainModels.Model.Mongo
{
    public interface IEntity 
    {
        [BsonId]
        string Id { get; set; }    
    }
}
