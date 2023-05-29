using System;
using catalogServiceAPI.Models;
using MongoDB.Driver;

namespace catalogServiceAPI.Services
{
	public class ItemToAuctionRepository : IItemToAuctionRepository
	{
        public readonly IConfiguration _config;

        public readonly ILogger<ItemToAuctionRepository> _logger;

        private readonly IMongoCollection<Item> _collection;

        public ItemToAuctionRepository(ILogger<ItemToAuctionRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _logger.LogInformation($"INFO: connecitonstring er: {_config["connectionString"]} ");
            var mongoClient = new MongoClient(_config["connectionString"]);
            var database = mongoClient.GetDatabase(_config["database"]);
            _collection = database.GetCollection<Item>(_config["collection"]);
        }

        //Metode til at sende items til AuctionService, til at oprette auktioner

        public List<ItemToAuction> PostItemToAuction()
        {
            _logger.LogInformation("INFO: Metoden SendItemsToAuction er kørt kl {DT}");
            
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


        //



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

        ItemToAuction[] IItemToAuctionRepository.PostItemToAuction()
        {
            throw new NotImplementedException();
        }

        /*private static void TimerElapsed()
        {

        }*/


        //Post

    }
}

