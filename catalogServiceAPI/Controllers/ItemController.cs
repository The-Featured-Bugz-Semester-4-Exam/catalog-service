using catalogServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace catalogServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    ItemsDBContext db = new ItemsDBContext();

    private readonly ILogger<ItemController> _logger;

    public ItemController(ILogger<ItemController> logger)
    {
        _logger = logger;
    }



    //Metode til at få alle Items ud af ItemsDB'en
    [HttpGet("AllItems")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public List<Item> GetAllItems()
    {
        try
        {
            _logger.LogInformation("Metode GetAllItems kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            var list = db.Items.Find(_ => true).ToList();
            _logger.LogInformation($"{list.First()}");
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode GetAllItems kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return null;
        }
    }


    
    //Metode til at få et specifikt Item ud af ItemsDB'en
    [HttpGet("{ID}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public Item GetItemOnID(int ID)
    {
        try
        {
            _logger.LogInformation("Metode GetItemOnID kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());
            var items = db.Items.Find(_ => true).ToList();
            var item = db.Items.Find(i => i.ItemID == ID).FirstOrDefault();
          
            foreach (var i in items)
            {
                if (i.ItemID == ID)
                {
                    item = i;
                }
            }
            _logger.LogInformation($"indhold af item: {item}");
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode GetItemOnID kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return null;
        }
    }

    



    [HttpPost(Name = "PostItem")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult PostItem([FromBody]Item item)
    {
        try
        {
            _logger.LogInformation("Metode PostItem kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            //_item.Add(item);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode PostItem kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return null;
        }
    }

}