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
            _logger.LogInformation($"connecitonstring er: {config["connectionString"]} ");
        }



        public List<Item> GetAllItems()
        {
            var list = _context.Items.Find(_ => true).ToList();
            _logger.LogInformation($"indhold af liste: {list}");
            return list;
        }



        public Item GetItemOnID(int ID)
        {
            var items = _context.Items.Find(_ => true).ToList();
            var item = items.FirstOrDefault(i => i.ItemID == ID);
            _logger.LogInformation($"Indhold af item: {item}");
            return item;
        }



        public void PostItem(Item item)
        {
            _logger.LogInformation($"Indhold af item til post: {item}");
            _context.Items.InsertOne(item);
        }



        public bool DeleteItem(int ID)
        {
            throw new NotImplementedException("not implementet");
        }



        public bool UpdateItem(Item item)
        {
            throw new NotImplementedException("not implementet");
        }
    }
}

