using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //Pizza
        public List<Pizza> Pizze { get; set; }

        public Ingredient() 
        {
        }
    }
}
