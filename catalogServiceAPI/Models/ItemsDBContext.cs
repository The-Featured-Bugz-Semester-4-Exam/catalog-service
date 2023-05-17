using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace catalogServiceAPI.Models
{
    public class ItemsDBContext
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly IMongoCollection<Item> mongoCollection;
        public ItemsDBContext()
        {
            string connectionString = "mongodb://localhost:27017/"; //Vores ConnectionString - Lige nu LocalHost
            var client = new MongoClient(connectionString);

            string databaseName = "ItemsDB"; //Vores DB - ItemsDB
            mongoDatabase = client.GetDatabase("ItemsDB");

            string collectionName = "Items"; // Vores Collection
            mongoCollection = mongoDatabase.GetCollection<Item>("Items");
        }

        public IMongoCollection<Item> Items
        {
            get
            {
                Console.WriteLine($"indhold af DB: {mongoCollection}");
                return mongoCollection;
            }
        }
    }
}

