using catalogServiceAPI.Models;
using catalogServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace catalogServiceAPI.Controllers;

[ApiController]
[Route("api")]
public class ItemController : ControllerBase
{
    private readonly HttpClient _httpClient = new HttpClient();

    private readonly ILogger<ItemController> _logger;

    private readonly IItemsRepository _repository;

    public ItemController(ILogger<ItemController> logger, IItemsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }


    //Metode til at få alle Items ud af ItemsDB'en

    [HttpGet("Get/All")]
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

    [HttpDelete("delete/{ID}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult DeleteItem(int ID)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.DeleteItem(ID);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode DeleteItem kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
        }
    }


    //Metode til at update et item

    [HttpPut("update/{ID}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult UpdateItem(int ID, Item item)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            bool isUpdated = _repository.UpdateItem(ID, item);

            if (isUpdated == true)
            {
                _logger.LogInformation($"SUCCES: item med ID {ID} blev modificeret");
                return Ok();
            }

            else
            {
                _logger.LogInformation($"FEJL: item med ID {ID} blev ikke modificeret");
                return StatusCode(StatusCodes.Status304NotModified);
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode DeleteItem kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
        }
    }


    //Metode til at Sende et item til AuctionService

    [HttpPost("PostAuction")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult PostAuction(int ID)
    {
        try
        {
            _logger.LogInformation("INFO: Metode PostAuction kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.GetItemOnID(ID);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode PostAuction kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
        }
       
        return Ok();
    }


    //Metode til at Sende et item til AuctionService
    [HttpPost("NewAuctions")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]

    public IActionResult PostItemToAuction(List<Item> items)
    {
        try
        {
            _logger.LogInformation("INFO: Metode PostItemToAuction kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            //opretter en liste kaldet 'auctions'
            List<ItemToAuction> auctions = new List<ItemToAuction>();

            var list = _repository.PostItemToAuction();

            //Hvis listen er tom
            if (list == null)
            {
                _logger.LogInformation("INFO: Listen er tom");
                return StatusCode(StatusCodes.Status204NoContent);
            }
            _logger.LogInformation($"INFO: Indhold af variabel list: {list}");

            //Loop der sætter 'auction' instanser ind i 'auctions'-listen
            foreach (Item item in list)
            {
                // Konverterer hvert 'Item' i list til det nye 'ItemToAuction' format
                ItemToAuction auction = new ItemToAuction(item);
                auctions.Add(auction);
            }

            return Ok(auctions);
        }
        //hvis andre fejl
        catch (HttpRequestException ex)
        {
            _logger.LogInformation("FEJL: Metode PostItemToAuction kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());
        }
        return StatusCode(StatusCodes.Status402PaymentRequired);
    }
}