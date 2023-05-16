using catalogServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace catalogServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    

    private readonly ILogger<ItemController> _logger;

    public ItemController(ILogger<ItemController> logger)
    {
        _logger = logger;
    }



    [HttpPost(Name = "PostItem")]
    public IEnumerable<Item> Get()
    {
       
    }

}