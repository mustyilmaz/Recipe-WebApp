using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;
using System.Security.Claims;

namespace RecipeApp.Pages.Recipe
{
    [Authorize]
    public class RatingModel : PageModel
    {
        private readonly RecipeAppContext _context;

        public RatingModel(RecipeAppContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        public IActionResult OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingRating = _context.Ratings.FirstOrDefault(r => r.RecipeId == id && r.UserId == userId);

            if (existingRating != null)
            {
                TempData["RatingMessage"] = "You have already voted for this recipe.";
                return RedirectToPage("/Recipe/Index", new { ratingMessage = TempData["RatingMessage"] });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int _rate)
        {
            var rating = new Rating();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                rating.UserId = userId;
                rating.Value = _rate;
                rating.RecipeId = id;
                _context.Ratings.Add(rating);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
