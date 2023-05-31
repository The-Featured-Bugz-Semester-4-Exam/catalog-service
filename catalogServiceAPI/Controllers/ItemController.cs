using catalogServiceAPI.Models;
using catalogServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
namespace catalogServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly HttpClient _httpClient = new HttpClient();

    public readonly IConfiguration _config;

    private readonly ILogger<ItemController> _logger;

    private readonly IItemsRepository _repository;

    public ItemController(IConfiguration config, ILogger<ItemController> logger, IItemsRepository repository)
    {
        _config = config;
        _logger = logger;
        _repository = repository;
    }




    [HttpGet("version")]
    public IEnumerable<string> GetVersion()
    {
        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;
        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()} \n");
        }
        return properties;
    }


    [HttpGet("getAllItems")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult GetAllItems()
    {
        try
        {
            _logger.LogInformation("SUCCES: Metode GetAllItems called {DT} did well," + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            var list = _repository.GetAllItems();

            return Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error: Metode GetAllItems called {DT}, going wrong" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status404NotFound);
        }
    }


    //Metode til at få et specifikt Item ud af ItemsDB'en

    [HttpGet("getItem/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult GetItemOnID(int id)
    {
        try
        {
            _logger.LogInformation("INFO: Metode GetItemOnID called {DT}",
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.GetItemOnID(id);

            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode GetItemOnID called {DT}, going wrong" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status404NotFound);
        }
    }



    [HttpPost("postItem")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult PostItem([FromBody] Item item)
    {
        try
        {
            _logger.LogInformation("SUCCES: Metode PostItem called {DT}, did fine" + StatusCodes.Status200OK,
            DateTime.UtcNow.ToLongTimeString());

            _repository.PostItem(item);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode PostItem called {DT}, going wrong" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status304NotModified);
        }
    }


    //Metode til at delete et item

    [HttpDelete("deleteItem/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult DeleteItem(int id)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem called {DT} with item ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.DeleteItem(id);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error: Metode DeleteItem called {DT}, going wrong" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status404NotFound);
        }
    }



    [HttpPut("updateItem/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult UpdateItem(int id, [FromBody] Item item)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem called {DT} with item ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            bool isUpdated = _repository.UpdateItem(id, item);

            if (isUpdated == true)
            {
                _logger.LogInformation($"SUCCES: item with ID {id} was modified");
                return Ok();
            }

            else
            {
                _logger.LogInformation($"Error: item with ID {id} was not modified");
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error: Metode DeleteItem called {DT}, going wrong",
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
        }
    }



    [HttpPost("postItemsToAuction")]
    public async Task<IActionResult> PostItemsToAuction([FromBody] ItemToAuction[] itemToAuctionList)
    {
        var json = JsonConvert.SerializeObject(itemToAuctionList);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
            _logger.LogInformation($"INFO: Trying to Post to auction-service: {_config["connAuk"]}/auction/postAuctions");
            var response = await httpClient.PostAsync($"{_config["connAuk"]}/auction/postAuctions", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SUCCES: Items successfully sent to the auction service.");
                return Ok();
            }
            else
            {
                _logger.LogError("Error: Failed to send items to the auction service.");
                return NotFound();
            }
        }
    }


    
    [HttpGet("getAuctionPrice/{id}")]
    public async Task<IActionResult> GetAuctionPrice(int id)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync($"{_config["connAuk"]}/auction/getAuctionPrice/{id}");
            string responseContent = await response.Content.ReadAsStringAsync();
            int price = Convert.ToInt16(responseContent);

            _logger.LogInformation($"INFO: env to auction-service: {_config["connAuk"]}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SUCCES: Price successfully sent from the auction service.");
                return Ok(price);
            }
            else
            {
                _logger.LogError("Error: Failed to get price from the auction service. ");
                return NotFound();
            }
        }
    }
}
