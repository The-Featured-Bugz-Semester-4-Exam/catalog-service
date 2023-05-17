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

    private readonly IItemsDBContext collection = new ItemsDBContext();

    private readonly ILogger<ItemController> _logger;

    public ItemController(ILogger<ItemController> logger)
    {
        _logger = logger;
    }







    //Metode til at få alle Items ud af ItemsDB'en

    [HttpGet("get/all")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public List<Item> GetAllItems()
    {
        try
        {
            _logger.LogInformation("SUCCES: Metode GetAllItems kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
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

    [HttpGet("Get/{ID}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult GetItemOnID(int ID)
    {
        try
        {
            _logger.LogInformation("INFO: Metode GetItemOnID kaldt {DT}",
            DateTime.UtcNow.ToLongTimeString());
            //var items = db.Items.Find(_ => true).ToList();
            //var item = db.Items.Find(i => i.ItemID == ID).FirstOrDefault();

            var item = db.Items.Find(i => i.ItemID == ID).FirstOrDefault();
            
            if (item == null)
            {
                _logger.LogInformation("FEJL: variabel 'item' har intet indhold" + StatusCodes.Status204NoContent,
                DateTime.UtcNow.ToLongTimeString());

                return NoContent();
            }
            _logger.LogInformation("SUCCES: Metode GetItemOnID returnerede item" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            return Ok(item);


            /*
            foreach (var i in items)
            {
                if (i.ItemID == ID)
                {
                    item = i;
                }
            }
            _logger.LogInformation($"INFO: indhold af item: {item}");
            return item;*/
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode GetItemOnID kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return NotFound();
        }
    }








    [HttpPost("PostItem")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult PostItem([FromBody] Item item)
    {
        try
        {
            _logger.LogInformation("SUCCES: Metode PostItem kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
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








    [HttpDelete("Delete/{ID}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult DeleteItem(int ID)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem kaldt {DT}" +
            DateTime.UtcNow.ToLongTimeString());

            var objectID = ID;
            var filter = Builders<Item>.Filter.Eq("ItemID", objectID);

            Console.WriteLine($"hvad er i filteret?: {filter}");
            Console.WriteLine($"er filteret null?: {filter != null}");


            if (filter == null)
            {
                _logger.LogInformation($"FEJL: Item med ItemID: {ID} ikke fundet" + StatusCodes.Status404NotFound,
                DateTime.UtcNow.ToLongTimeString());

                return NotFound();
            }

            _logger.LogInformation($"INFO: Item med ItemID: {ID} fundet",
            DateTime.UtcNow.ToLongTimeString());

            var result = db.Items.DeleteOne(filter);

            if (result.DeletedCount > 0)
            {
                _logger.LogInformation($"SUCCES: Item med ItemID: {ID} slettet" + StatusCodes.Status200OK,
                DateTime.UtcNow.ToLongTimeString());

                return Ok();
            }

            _logger.LogInformation("FEJL: Metode DeleteItem kaldt {DT}, den blev ikke slettet",
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status417ExpectationFailed);

        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode DeleteItem kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


}