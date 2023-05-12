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
        public IActionResult Index(string? search)
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizza;
                pizza = db.pizze.Include(pizza => pizza.Ingredients).ToList<Pizza>();

                if(search != null)
                {
                    pizza = pizza.Where(p => p.Nome.ToLower().Contains(search.ToLower())).ToList();
                }
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

        [HttpPut ("{id}")]
        public IActionResult Edit(int Id, [FromBody] Pizza data)
        {

            using PizzaContext db = new PizzaContext();
            Pizza pizzaEdit = db.pizze.Include(p => p.Ingredients).FirstOrDefault(m => m.Id == Id);

            pizzaEdit.Ingredients.Clear();

            if (pizzaEdit != null)
            {

                pizzaEdit.Nome = data.Nome;
                pizzaEdit.Descrizione = data.Descrizione;
                pizzaEdit.Image = data.Image;
                pizzaEdit.Price = data.Price;
                pizzaEdit.CategoryId = data.CategoryId;

                if (data.Ingredients != null)
                {
                    foreach (Ingredient i in data.Ingredients)
                    {
                        int selectIntIngredientId = i.Id;
                        Ingredient ingredient = db.ingredients
                        .Where(m => m.Id == selectIntIngredientId)
                        .FirstOrDefault();
                        pizzaEdit.Ingredients.Add(ingredient);
                    }
                }

                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete ("{id}")]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaToDelete = db.pizze.Where(m => m.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                db.pizze.Remove(pizzaToDelete);
                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


    }

}
