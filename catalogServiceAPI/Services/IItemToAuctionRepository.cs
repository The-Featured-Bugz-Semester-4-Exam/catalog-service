using System;
using catalogServiceAPI.Models;

namespace catalogServiceAPI.Services
{
	public interface IItemToAuctionRepository
	{
        List<ItemToAuction> PostItemToAuction();
    }
}

