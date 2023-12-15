//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using RecipeApp.Models;
//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace RecipeApp.Pages.Recipe
//{
//    public class FavoriteRecipeModel : PageModel
//    {
//        private readonly RecipeAppContext _context;

//        public FavoriteRecipeModel(RecipeAppContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> OnGetAsync(int id)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
//            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);

//            if (recipe != null)
//            {
                
//                var favoriteRecipe = new FavoriteRecipe
//                {
//                    RecipeId = recipe.Id,
//                    UserId = userId,
//                };

             
//                _context.FavoriteRecipe.Add(favoriteRecipe);
//                await _context.SaveChangesAsync();
//            }

//            // Ana tarif sayfasýna yönlendirin
//            return RedirectToPage("Index");
//        }
//    }
//}
