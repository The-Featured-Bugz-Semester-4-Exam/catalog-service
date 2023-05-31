using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace catalogServiceAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MongoId { get; set; }

        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemStartPrice { get; set; }
        public int ItemCurrentBid { get; set; } = -1;
        public int ItemSellerID { get; set; }
        public DateTime ItemStartDate { get; set; }
        public DateTime ItemEndDate { get; set; }
    }
}

