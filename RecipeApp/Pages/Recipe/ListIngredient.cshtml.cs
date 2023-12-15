using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;
using System.Collections.Generic;

namespace RecipeApp.Pages.Recipe
{
    public class ListIngredientModel : PageModel
    {

        private readonly RecipeAppContext _context;

        [BindProperty]
        public Ingredient Ingredient { get; set; }

        public ListIngredientModel(RecipeAppContext context)
        {
            _context = context;
        }
        public List<Ingredient> Ingredients { get; set; }
        public int RecipeId { get; set; }


        public async Task<IActionResult> OnGetAsync(int recipeId)
        {
            Ingredients = await _context.Ingredients
                .Where(i => i.RecipeId == recipeId)
                .ToListAsync();

            RecipeId = recipeId;

            if (Ingredients == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
