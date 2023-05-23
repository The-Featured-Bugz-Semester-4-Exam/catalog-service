using System;
using catalogServiceAPI.Models;

namespace catalogServiceAPI.Services
{
	public interface IItemsRepository
	{
        List<Item> GetAllItems();
        Item GetItemOnID(int ItemID);
        void PostItem(Item item);
        bool UpdateItem(int ItemID, Item item);
        bool DeleteItem(int ItemID);
        List<Item> PostItemToAuction();
    }
}

