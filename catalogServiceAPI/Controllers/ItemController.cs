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
    private readonly ILogger<ItemController> _logger;

    private readonly IItemsRepository _repository;

    public ItemController(ILogger<ItemController> logger, IItemsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }


    //Metode til at få alle Items ud af ItemsDB'en

    [HttpGet("get/all")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult GetAllItems()
    {
        try
        {
            _logger.LogInformation("SUCCES: Metode GetAllItems kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            var list = _repository.GetAllItems();

            return Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode GetAllItems kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
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

            var item = _repository.GetItemOnID(ID);

            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode GetItemOnID kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
        }
    }


    //Metode til at oprette et item i cataloget

    [HttpPost("PostItem")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult PostItem([FromBody] Item item)
    {
        try
        {
            _logger.LogInformation("SUCCES: Metode PostItem kaldt {DT}, det gik fint" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            _repository.PostItem(item);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode PostItem kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status304NotModified);
        }
    }


    //Metode til at delete et item

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

            throw new NotImplementedException("under ombygning");
            /*
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
            */
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode DeleteItem kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


}