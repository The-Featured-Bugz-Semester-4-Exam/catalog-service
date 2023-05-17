using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace catalogServiceAPI.Models
{
	public class ItemsRepository : IItemsRepository
	{
        ItemsDBContext db = new ItemsDBContext();

        public List<Item> GetAllItems()
        {
            throw new NotImplementedException("not implementet");
        }

        public Item GetItemOnID(int ID)
        {
            throw new NotImplementedException("not implementet");
        }

        public void PostItem(Item item)
        {
            throw new NotImplementedException("not implementet");
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

