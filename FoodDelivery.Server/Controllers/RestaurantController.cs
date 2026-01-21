using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Models.RestaurantModels;


namespace FoodDelivery.Server.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Id = 1,
                    Name = "Pizza Hut",
                    ImageUrl = "/images/restaurants/PizzaHut.svg",
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Burger King",
                    ImageUrl = "/images/restaurants/BurgerKing.svg",
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "Hotel Hasara",
                    ImageUrl = "/images/restaurants/HotelHasara.svg",
                },
                new Restaurant
                {
                    Id = 4,
                    Name = "KFC",
                    ImageUrl = "/images/restaurants/KFC.svg",
                },
                new Restaurant
                {
                    Id = 5,
                    Name = "Madeena Beach Hotel",
                    ImageUrl = "/images/restaurants/Madeena.svg",
                },
            };
            return Ok(restaurants);
        }
    }
}