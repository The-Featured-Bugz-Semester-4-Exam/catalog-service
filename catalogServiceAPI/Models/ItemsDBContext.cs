using System;
using MongoDB.Bson;
using MongoDB.Driver;
using catalogServiceAPI.Controllers;

namespace catalogServiceAPI.Models
{
    public class ItemsDBContext
    {
        public readonly IMongoDatabase mongoDatabase;
        public readonly IMongoCollection<Item> mongoCollection;
        public ItemsDBContext()
        {
            string connectionString = "mongodb://localhost:27017/"; //Vores ConnectionString - Lige nu LocalHost
            var client = new MongoClient(connectionString);

            string databaseName = "ItemsDB"; //Vores DB - ItemsDB
            var mongoDatabase = client.GetDatabase("ItemsDB");

            string collectionName = "Items"; // Vores Collection
            var mongoCollection = mongoDatabase.GetCollection<Item>("Items");
        }

        public IMongoCollection<Item> Items
        {
            get
            {
                Console.WriteLine($"indhold af DB: {mongoCollection}");
                return mongoDatabase.GetCollection<Item>("Items");
            }
        }
    }
}

