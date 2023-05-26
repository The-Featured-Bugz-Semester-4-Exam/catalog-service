﻿using catalogServiceAPI.Models;
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

namespace catalogServiceAPI.Controllers;

[ApiController]
[Route("api")]
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


    //Metode til at få alle Items ud af ItemsDB'en

    [HttpGet("getAllItems")]
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

    [HttpGet("getItem/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult GetItemOnID(int id)
    {
        try
        {
            _logger.LogInformation("INFO: Metode GetItemOnID kaldt {DT}",
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.GetItemOnID(id);

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

    [HttpPost("postItem")]
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

    [HttpDelete("deleteItem/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult DeleteItem(int id)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.DeleteItem(id);

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

    [HttpPut("updateItem/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult UpdateItem(int id,[FromBody] Item item)
    {
        try
        {
            _logger.LogInformation("INFO: Metode DeleteItem kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            bool isUpdated = _repository.UpdateItem(id, item);

            if (isUpdated == true)
            {
                _logger.LogInformation($"SUCCES: item med ID {id} blev modificeret");
                return Ok();
            }

            else
            {
                _logger.LogInformation($"FEJL: item med ID {id} blev ikke modificeret");
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

    [HttpPost("postAuction/{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    public IActionResult PostAuction(int id)
    {
        try
        {
            _logger.LogInformation("INFO: Metode PostAuction kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            var item = _repository.GetItemOnID(id);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("FEJL: Metode PostAuction kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());

            return StatusCode(StatusCodes.Status418ImATeapot);
        }
       
        return Ok();
    }

/*
    //Metode til at Sende et item til AuctionService
    [HttpPost("newAuctions")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]

    public IActionResult PostItemToAuction(List<Item> items)
    {
        try
        {
            _logger.LogInformation("INFO: Metode PostItemToAuction kaldt {DT} på Item med ID {ID}" +
            DateTime.UtcNow.ToLongTimeString());

            //opretter en liste kaldet 'auctions'
            List<ItemToAuction> auctions = new List<ItemToAuction>();

            var list = _repository.GetAllItems();
                //PostItemToAuction();

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

            return Ok();
        }
        //hvis andre fejl
        catch (HttpRequestException ex)
        {
            _logger.LogInformation("FEJL: Metode PostItemToAuction kaldt {DT}, det gik galt" + ex,
            DateTime.UtcNow.ToLongTimeString());
        }
        return StatusCode(StatusCodes.Status402PaymentRequired);
    }*/

    [HttpPost("postItemsToAuction")]
    public async Task<IActionResult> PostItemsToAuction([FromBody] ItemToAuction[] itemToAuctionList)
    {
        var json = JsonConvert.SerializeObject(itemToAuctionList);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsync($"{_config["connAuk"]}/api/postAuctions", content);
            _logger.LogInformation($"INFO: env til auction-service: {_config["connAuk"]}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SUCCES: Items successfully sent to the auction service.");
                return Ok();
            }
            else
            {
                _logger.LogError("FEJL: Failed to send items to the auction service.");
                return NotFound();
            }
        }
    }


    //Metode til at hente en pris
    [HttpGet("getAuctionPrice/{id}")]
    public async Task<IActionResult> GetAuctionPrice(int id)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync($"{_config["connAuk"]}/api/getAuctionPrice/{id}");
            string responseContent = await response.Content.ReadAsStringAsync();
            int price = Convert.ToInt16(responseContent);

            _logger.LogInformation($"INFO: env til auction-service: {_config["connAuk"]}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SUCCES: Price successfully sent from the auction service.");
                return Ok(price);
            }
            else
            {
                _logger.LogError("FEJL: Failed to get price from the auction service. ");
                return NotFound();
            }
        }
    }
}