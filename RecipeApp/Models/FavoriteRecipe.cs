using System;
using System.Collections.Generic;
namespace RecipeApp.Models
{
    public class FavoriteRecipe
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; }
    }
}
