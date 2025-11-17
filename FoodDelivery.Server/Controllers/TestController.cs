using Microsoft.AspNetCore.Mvc;


namespace FoodDelivery.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController
    {
        [HttpGet]
        public string Get() => "Backend Ok!";
    }
}