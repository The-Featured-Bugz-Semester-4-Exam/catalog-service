using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using catalogServiceAPI.Controllers;

namespace catalogServiceAPI.Models
{
    public class ItemsRepository : IItemsRepository
    {
        public readonly IConfiguration _config;

        public readonly ILogger<ItemsRepository> _logger;

        private readonly ItemsDBContext _context;

        public ItemsRepository(ILogger<ItemsRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _context = new ItemsDBContext(config);
            _logger.LogInformation($"INFO: connecitonstring er: {config["connectionString"]} ");
        }


        //Metode til at hente alle items

        public List<Item> GetAllItems()
        {
            var list = _context.Items.Find(_ => true).ToList();
            _logger.LogInformation($"INFO: indhold af liste: {list}");
            return list;
        }


        //Metode til at hente et item på ID

        public Item GetItemOnID(int ID)
        {
            var items = _context.Items.Find(_ => true).ToList();
            var item = items.FirstOrDefault(i => i.ItemID == ID);
            _logger.LogInformation($"INFO: Indhold af item: {item}");
            return item;
        }


        //Metode til at oprette et item i Postman med JSON

        public void PostItem(Item item)
        {
            _logger.LogInformation($"INFO: Indhold af item til post: {item}");
            _context.Items.InsertOne(item);
        }


        //Metode til at slette et item på ID eller dø i forsøget

        public bool DeleteItem(int ID)
        {
            _logger.LogInformation($"INFO: ID på item flagged for delete: {ID}");
            var filter = Builders<Item>.Filter.Eq(item => item.ItemID, ID);
            var result = _context.Items.DeleteOne(filter);
            _context.Items.DeleteOne(filter);

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
            var existingItem = _context.Items.Find(filter).FirstOrDefault();

            if (existingItem != null)
            {
                existingItem.ItemName = updatedItem.ItemName;
                existingItem.ItemDescription = updatedItem.ItemDescription;
                existingItem.ItemStartPrice = updatedItem.ItemStartPrice;
                existingItem.ItemSellerID = updatedItem.ItemSellerID;
                existingItem.ItemStartDate = updatedItem.ItemStartDate;
                existingItem.ItemEndDate = updatedItem.ItemEndDate;

                var result = _context.Items.ReplaceOne(filter, existingItem);
                bool isUpdated = result.ModifiedCount > 0;

                return isUpdated;
            }

            else
            {
                return false;
            }

        }


        //Metode til at sende items til BidService, til at oprette auktioner

        public void SendItemsToAuction()
        {
            _logger.LogInformation("INFO: Metoden SendItemsToAuction er kørt kl {DT");

            DateTime currentDT = DateTime.UtcNow;

            var ItemsToSend = _context.Items.Find(i => i.ItemStartDate < currentDT).ToList();
        }


        //Metode til at fjerne items, der har været på auktion

        public void RemoveExpiredItems()
        {
            _logger.LogInformation("INFO: Metoden RemoveExpiredItems er kørt kl {DT}");

            DateTime currentDT = DateTime.UtcNow;
            DateTime nextMidnight = currentDT.Date.AddDays(1).AddHours(0);

            TimeSpan timeUntilNextMidnight = nextMidnight - currentDT;

            var expiredItems = _context.Items.Find(i => i.ItemEndDate < currentDT).ToList();

            foreach (var item in expiredItems)
            {
                _context.Items.DeleteOne(i => i.ItemID == item.ItemID)
            }
        }


        //Metode til at styre cleanup af udløbne items 

        public void ScheduledTimer()
        {
            //Timer der kører metoden en gang om dagen
            Timer timer = new Timer(RemoveExpiredItems, null, TimeSpan.Zero, TimeSpan.FromHours(24))
        }
    }
}

