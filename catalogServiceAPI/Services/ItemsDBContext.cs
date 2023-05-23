using System;
using MongoDB.Bson;
using MongoDB.Driver;
using catalogServiceAPI.Controllers;
using catalogServiceAPI.Models;

namespace catalogServiceAPI.Services
{
    public class ItemsDBContext
    {
        private readonly IConfiguration _config;

        public ItemsDBContext(IConfiguration config)
        {
            _config = config;
        }

        public IMongoCollection<Item> Items
        {
            get
            {
                string connectionString = _config["connectionString"]; //Vores ConnectionString - Lige nu LocalHost
                var client = new MongoClient(connectionString);

                string databaseName = "ItemsDB"; //Vores DB - ItemsDB
                var mongoDatabase = client.GetDatabase("ItemsDB");

                string collectionName = "Items"; // Vores Collection
                return mongoDatabase.GetCollection<Item>("Items");
            }
        }
    }
}

