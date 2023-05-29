using System;
namespace catalogServiceAPI.Models
{
	public class ItemToAuction
	{
		public int? ItemID { get; set; }
        public int? ItemStartPrice { get; set; }
        public DateTime? ItemEndDate { get; set; }

        public ItemToAuction(Item item)
		{
			ItemID = item.ItemID;
			ItemStartPrice = item.ItemStartPrice;
			ItemEndDate = item.ItemEndDate;
		}
		public ItemToAuction()
		{
			
		}
	}
}

