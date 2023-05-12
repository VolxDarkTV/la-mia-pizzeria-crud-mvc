using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace la_mia_pizzeria_static.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuApiController : ControllerBase
    {

        [HttpGet]
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizza;
                pizza = db.pizze.Include(pizza => pizza.Ingredients).ToList<Pizza>();
                return Ok(pizza);
            }
        }

        

        [HttpPost]
        public IActionResult Create(Pizza data)
        {

            using PizzaContext db = new PizzaContext();
            Pizza pizzaToCreate = new Pizza();
            pizzaToCreate.Ingredients = new List<Ingredient>();
            pizzaToCreate.Nome = data.Nome;
            pizzaToCreate.Descrizione = data.Descrizione;
            pizzaToCreate.Image = data.Image;
            pizzaToCreate.Price = data.Price;
            pizzaToCreate.CategoryId = data.CategoryId;

            if (data.Ingredients != null)
            {
                foreach (Ingredient i in data.Ingredients)
                {
                    int selectIntIngredientId = i.Id;
                    Ingredient ingredient = db.ingredients.FirstOrDefault(m => m.Id == selectIntIngredientId);
                    pizzaToCreate.Ingredients.Add(ingredient);
                }
            }
            db.pizze.Add(pizzaToCreate);
            db.SaveChanges();

            return Ok();
        }

    }
}
