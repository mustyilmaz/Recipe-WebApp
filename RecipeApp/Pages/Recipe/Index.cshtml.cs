using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RecipeApp.Models.RecipeAppContext _context;

        public IndexModel(RecipeApp.Models.RecipeAppContext context)
        {
            _context = context;
        }

        public IList<Models.Recipe> Recipe { get;set; }

        public List<double>? RecipeAverageRatingList { get; set; } = default!;

        

        public async Task<Dictionary<int, double>> CalculateRatings()
        {
            var ratingsByRecipe = new Dictionary<int, double>();

            var allRatings = await _context.Ratings.ToListAsync();

            foreach (var recipe in Recipe)
            {
                var recipeRatings = allRatings.Where(r => r.RecipeId == recipe.Id).Select(r => r.Value).ToList();

                if (recipeRatings.Count >= 1)
                {
                    double avg = AvgRate.GetRating(recipeRatings);
                    avg = Math.Round(avg, 1);
                    ratingsByRecipe.Add(recipe.Id, avg);
                }
                else
                {
                    ratingsByRecipe.Add(recipe.Id, 0.0);
                }
            }

            return ratingsByRecipe;
        }

        public string userId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Cuisine { get; set; }

        public SelectList Cuisines { get; set; }

        public Dictionary<int, double> RecipeAverageRatingDictionary { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ratingMessage { get; set; }
        public string RatingMessage { get; set; }

        
        public async Task OnGetAsync()
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<string> cuisineQuery = from r in _context.Recipes
                                              orderby r.Cuisine
                                              select r.Cuisine;
            var recipes = from r in _context.Recipes
                          select r;
            RatingMessage = ratingMessage;
            if (_context.Recipes != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    recipes = recipes.Where(s => s.Title.Contains(SearchString));
                }
                if (!string.IsNullOrEmpty(Cuisine))
                {
                    recipes = recipes.Where(x => x.Cuisine == Cuisine);
                }
                
            }
            Recipe = await recipes.OrderBy(r => r.Id).ToListAsync();
            
            Cuisines = new SelectList(await cuisineQuery.Distinct().ToListAsync());

            RecipeAverageRatingDictionary = await CalculateRatings();
        }
    }
}
