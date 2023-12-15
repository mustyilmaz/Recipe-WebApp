using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe
{
    [Authorize]
    public class IngredientModel : PageModel
    {
        private readonly RecipeAppContext _context;

        [BindProperty]
        public Ingredient Ingredient { get; set; }

        public IngredientModel(RecipeAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int recipeId)
        {
            Ingredient = new Ingredient { RecipeId = recipeId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Ingredients.Add(Ingredient);
            await _context.SaveChangesAsync();

            return RedirectToPage("Details", new { id = Ingredient.RecipeId });
        }
    }
}
