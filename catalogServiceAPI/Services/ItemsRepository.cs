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
            _logger.LogInformation($"INFO: connecitonstring er: {config["connectionString"]} ");
            var mongoClient = new MongoClient(_config["connectionString"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _collection = database.GetCollection<Item>(_config["collection"]);
        }


        public List<Item> GetAllItems()
        {
            var list = _collection.Find(_ => true).ToList();
            _logger.LogInformation($"INFO: indhold af liste: {list}");
            return list;
        }


        //Metode til at hente et item på ID

        public Item GetItemOnID(int ID)
        {
            var items = _collection.Find(_ => true).ToList();
            var item = items.FirstOrDefault(i => i.ItemID == ID);
            _logger.LogInformation($"INFO: Indhold af item: {item}");
            return item;
        }


        //Metode til at oprette et item i Postman med JSON

        public void PostItem(Item item)
        {
            _logger.LogInformation($"INFO: Indhold af item til post: {item}");
            _collection.InsertOne(item);
        }


        //Metode til at slette et item på ID eller dø i forsøget

        public bool DeleteItem(int ID)
        {
            _logger.LogInformation($"INFO: ID på item flagged for delete: {ID}");
            var filter = Builders<Item>.Filter.Eq(item => item.ItemID, ID);
            var result = _collection.DeleteOne(filter);
            _collection.DeleteOne(filter);

            if (result.DeletedCount == 1)
            {
                _logger.LogInformation($"SUCCES: Item med ID {ID} er blevet slettet");
                return true;
            }
            else
            {
                _logger.LogInformation($"FEJL: Item med ID'et {ID} overlevede og kommer nu efter dig for hævn");
                return false;
            }
        }


        //Metode til at opdaterer et item

        public bool UpdateItem(int ID, Item updatedItem)
        {

            _logger.LogInformation($"INFO: ID på item flagged til opdatering: {ID}");
            var filter = Builders<Item>.Filter.Eq(i => i.ItemID, ID);
            var existingItem = _collection.Find(filter).FirstOrDefault();

            if (existingItem != null)
            {
                existingItem.ItemName = updatedItem.ItemName;
                existingItem.ItemDescription = updatedItem.ItemDescription;
                existingItem.ItemStartPrice = updatedItem.ItemStartPrice;
                existingItem.ItemCurrentBid = updatedItem.ItemCurrentBid;
                existingItem.ItemSellerID = updatedItem.ItemSellerID;
                existingItem.ItemStartDate = updatedItem.ItemStartDate;
                existingItem.ItemEndDate = updatedItem.ItemEndDate;

                var result = _collection.ReplaceOne(filter, existingItem);
                bool isUpdated = result.ModifiedCount > 0;

                return isUpdated;
            }

            else
            {
                return false;
            }
        }


        //Metode til at sende items til AuctionService, til at oprette auktioner

        public List<ItemToAuction> PostItemToAuction()
        {
            _logger.LogInformation("INFO: Metoden SendItemsToAuction er kørt kl {DT");

            DateTime currentDT = DateTime.UtcNow;

            var itemList = _collection.Find(i => i.ItemStartDate < currentDT).ToList();
            var itemToAuctionList = new List<ItemToAuction>();

            foreach (var item in itemList)
            {
                var itemToAuction = new ItemToAuction(item);
                itemToAuctionList.Add(itemToAuction);
            }

            _logger.LogInformation($"INFO: Indhold af variabel 'list': {string.Join(", ", itemToAuctionList)}");

            return itemToAuctionList;
        }


        //Metode til at styre cleanup af udløbne items 
        
        private static System.Timers.Timer timer;

        public void ScheduledTimer()
        {
            //Timer der kører metoden en gang pr 12 time (ms * s * m * t)
            int intervalInMilliseconds = 1000 * 60 * 60 * 12;
            DateTime now = DateTime.Now;

            timer = new System.Timers.Timer(intervalInMilliseconds);

            //timer.Elapsed += TimerElapsed();

            timer.Start();

            _logger.LogInformation($"timer startet: {now} - tid til næste interval: {intervalInMilliseconds}");
        }

        /*private static void TimerElapsed()
        {

        }*/
        

        //Post

    }
}

