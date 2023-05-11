using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    public class MenuController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? message)
        {
            using (PizzaContext db = new PizzaContext())
            {
                if(message != null)
                    ViewData["Message"] = message;
                List<Pizza> pizza = db.pizze.ToList<Pizza>();
                return View(pizza);
            }
        }

        [Authorize (Roles = "ADMIN, USER")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizza = db.pizze
                .Include(pizza => pizza.Category)
                .Include(pizza => pizza.Ingredients)
                .FirstOrDefault(m => m.Id == id);

            if (pizza == null)
                return NotFound($"La pizza con id {id} non è stata trovata");
            else
                return View("Show", pizza);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            using PizzaContext db = new PizzaContext();
            List<Category> categories = db.categories.ToList();

            PizzaFormModel model = new PizzaFormModel();
            model.Pizza = new Pizza();
            model.Categories = categories;

            List<Ingredient> ingredients = db.ingredients.ToList();
            List<SelectListItem> listIngredients = new List<SelectListItem>();

            foreach (Ingredient ingredient in ingredients)
            {
                listIngredients.Add(
                        new SelectListItem() { 
                            Text = ingredient.Name, Value = ingredient.Id.ToString() 
                        }
                );
            }
            model.Ingredients = listIngredients;
            return View("Create", model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext context = new PizzaContext();
                List<Category> category = context.categories.ToList();
                data.Categories = category;

                List<Ingredient> ingredients = context.ingredients.ToList();
                List<SelectListItem> listIngredients = new List<SelectListItem>();

                foreach (Ingredient ingredient in ingredients)
                {
                    listIngredients.Add
                    (
                        new SelectListItem()
                        {
                            Text = ingredient.Name,
                            Value = ingredient.Id.ToString()
                        }
                    );
                }
                data.Ingredients = listIngredients;
                return View("Create", data);
            }

            using PizzaContext db = new PizzaContext();
            Pizza pizzaToCreate = new Pizza();
            pizzaToCreate.Ingredients = new List<Ingredient>();
            pizzaToCreate.Nome = data.Pizza.Nome;
            pizzaToCreate.Descrizione = data.Pizza.Descrizione;
            pizzaToCreate.Image = data.Pizza.Image;
            pizzaToCreate.Price = data.Pizza.Price;
            pizzaToCreate.CategoryId = data.Pizza.CategoryId;

            if (data.SelectedIngredients != null)
            {
                foreach (string selectedIngredientId in data.SelectedIngredients)
                {
                    int selectIntIngredientId = int.Parse(selectedIngredientId);
                    Ingredient ingredient = db.ingredients.FirstOrDefault(m => m.Id == selectIntIngredientId);
                    pizzaToCreate.Ingredients.Add(ingredient);
                }
            }
            db.pizze.Add(pizzaToCreate);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        //Edit Method
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaEdit = db.pizze.Include(p => p.Ingredients).FirstOrDefault(pizza => pizza.Id == Id);
            if(pizzaEdit == null)
            {
                return NotFound();
            }
            else
            {
                List<Category> category = db.categories.ToList();
                PizzaFormModel model = new PizzaFormModel();
                model.Pizza = pizzaEdit;
                model.Categories = category;
                model.SelectedIngredients = new List<string>();
                //Aggiungo alla SelectedIngredients gli Id degli elementi
                foreach(var ingredient in pizzaEdit.Ingredients)
                {
                    model.SelectedIngredients.Add(ingredient.Id.ToString());
                }


                List<Ingredient> ingredients = db.ingredients.ToList();
                List<SelectListItem> listIngredients = new List<SelectListItem>();
                foreach (Ingredient ingredient in ingredients)
                {
                    listIngredients.Add(
                        new SelectListItem()
                        {
                            Text = ingredient.Name,
                            Value = ingredient.Id.ToString(),
                            Selected = pizzaEdit.Ingredients.Any(m => m.Id == ingredient.Id)
                        }
                    );
                }
                model.Ingredients = listIngredients;
                return View(model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int Id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext context = new PizzaContext();
                List<Category> category = context.categories.ToList();
                data.Categories = category;

                List<Ingredient> ingredients = context.ingredients.ToList();
                List<SelectListItem> listIngredients = new List<SelectListItem>();
                foreach (Ingredient ingredient in ingredients)
                {
                    listIngredients.Add(
                        new SelectListItem()
                        {
                            Text = ingredient.Name,
                            Value = ingredient.Id.ToString(),
                        }
                    );
                }

                data.Ingredients = listIngredients;

                return View("Edit", data);
            }

            using PizzaContext db = new PizzaContext();
            Pizza pizzaEdit = db.pizze.Include(p => p.Ingredients).FirstOrDefault(m => m.Id == Id);

            pizzaEdit.Ingredients.Clear();

            if(pizzaEdit != null)
            {

                pizzaEdit.Nome = data.Pizza.Nome;
                pizzaEdit.Descrizione = data.Pizza.Descrizione;
                pizzaEdit.Image = data.Pizza.Image;
                pizzaEdit.Price = data.Pizza.Price;
                pizzaEdit.CategoryId = data.Pizza.CategoryId;

                if (data.SelectedIngredients != null)
                {
                    foreach (string selectedIngredientId in data.SelectedIngredients)
                    {
                        int selectIntIngredientId = int.Parse(selectedIngredientId);
                        Ingredient ingredient = db.ingredients
                        .Where(m => m.Id == selectIntIngredientId)
                        .FirstOrDefault();
                        pizzaEdit.Ingredients.Add(ingredient);
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }



        //Delete Method
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaToDelete = db.pizze.Where(m => m.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                db.pizze.Remove(pizzaToDelete);
                db.SaveChanges();

                return RedirectToAction("Index", new {message = "Piatto Eliminato con successo!"});
            }
            else
            {
                return NotFound();
            }
        }
        
    }
}
