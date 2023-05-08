﻿using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class MenuController : Controller
    {
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

        public IActionResult Details(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizza = db.pizze.FirstOrDefault(m => m.Id == id);
            return View("Show", pizza);
        }


        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", pizza);
            }

            using PizzaContext db = new PizzaContext();
            Pizza pizzaToCreate = new Pizza();
            pizzaToCreate.Nome = pizza.Nome;
            pizzaToCreate.Descrizione = pizza.Descrizione;
            pizzaToCreate.Image = pizza.Image;
            pizzaToCreate.Price = pizza.Price;

            db.pizze.Add(pizzaToCreate);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Delete Method
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
