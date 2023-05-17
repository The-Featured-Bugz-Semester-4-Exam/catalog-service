using System;
using catalogServiceAPI.Models;

namespace catalogServiceAPI.Models
{
	public interface IItemsRepository
	{
        List<Item> GetAllItems();
        Item GetItemOnID(int ItemID);
        void PostItem(Item item);
        bool UpdateItem(Item item);
        bool DeleteItem(int ItemID);
    }
}

