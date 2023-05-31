using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using catalogServiceAPI.Controllers;
using catalogServiceAPI.Models;
using System.Text;
using System.Threading.Channels;
using MongoDB.Driver.Core.Bindings;
using System.Timers;

namespace catalogServiceAPI.Services
{
    public class ItemsRepository : IItemsRepository
    {
        public readonly IConfiguration _config;
        public readonly ILogger<ItemsRepository> _logger;
        private readonly IMongoCollection<Item> _collection;

        public ItemsRepository(ILogger<ItemsRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            // Retrieve the connection string from the configuration
            _logger.LogInformation($"INFO: connection string is: {config["connectionString"]}");

            // Create a new instance of MongoClient and get the database and collection
            var mongoClient = new MongoClient(_config["connectionString"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _collection = database.GetCollection<Item>(_config["collection"]);
        }

        public List<Item> GetAllItems()
        {
            // Find all items in the collection and convert to a list
            var list = _collection.Find(_ => true).ToList();
            _logger.LogInformation($"INFO: The Item collection: {list}");
            return list;
        }

        public Item GetItemOnID(int ID)
        {
            // Find all items in the collection and retrieve the item with the matching ID
            var items = _collection.Find(_ => true).ToList();
            var item = items.FirstOrDefault(i => i.ItemID == ID);
            _logger.LogInformation($"INFO: Item data: {item}");
            return item;
        }

        public void PostItem(Item item)
        {
            // Log the data of the item being posted
            _logger.LogInformation($"INFO: Item post data: {item}");

            // Insert the item into the collection
            _collection.InsertOne(item);
        }

        public bool DeleteItem(int ID)
        {
            _logger.LogInformation($"INFO: Trying to delete item with ID: {ID}");

            // Create a filter to find the item with the matching ID
            var filter = Builders<Item>.Filter.Eq(item => item.ItemID, ID);

            // Delete the item from the collection
            var result = _collection.DeleteOne(filter);

            if (result.DeletedCount == 1)
            {
                _logger.LogInformation($"INFO: Success, item with ID {ID} is deleted");
                return true;
            }
            else
            {
                _logger.LogInformation($"INFO: Error, item with ID {ID} not found");
                return false;
            }
        }

        public bool UpdateItem(int ID, Item updatedItem)
        {
            _logger.LogInformation($"INFO: Trying to update item with ID: {ID}");

            // Create a filter to find the item with the matching ID
            var filter = Builders<Item>.Filter.Eq(i => i.ItemID, ID);

            // Find the existing item in the collection
            var existingItem = _collection.Find(filter).FirstOrDefault();

            if (existingItem != null)
            {
                // Update the properties of the existing item with the new values
                existingItem.ItemName = updatedItem.ItemName;
                existingItem.ItemDescription = updatedItem.ItemDescription;
                existingItem.ItemStartPrice = updatedItem.ItemStartPrice;
                existingItem.ItemCurrentBid = updatedItem.ItemCurrentBid;
                existingItem.ItemSellerID = updatedItem.ItemSellerID;
                existingItem.ItemStartDate = updatedItem.ItemStartDate;
                existingItem.ItemEndDate = updatedItem.ItemEndDate;

                // Replace the existing item with the updated item in the collection
                var result = _collection.ReplaceOne(filter, existingItem);

                // Check if the item was successfully updated
                bool isUpdated = result.ModifiedCount > 0;

                _logger.LogInformation($"INFO: Success with updating item with ID {ID}");
                return isUpdated;
            }
            else
            {
                _logger.LogInformation($"INFO: Error with updating item with ID {ID}, item not found");
                return false;
            }
        }
    }
}

