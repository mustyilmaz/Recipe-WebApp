using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly RecipeApp.Models.RecipeAppContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(RecipeApp.Models.RecipeAppContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Comments = new List<Comment>();
        }

        public Models.Recipe Recipe { get; set; } = default!;
        public List<Comment> Comments { get; set; }
        [BindProperty]
        public Comment NewComment { get; set; }

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null || _context.Recipes == null)
        //    {
        //        return NotFound();
        //    }

        //    var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }
        //    else 
        //    {
        //        Recipe = recipe;
        //    }

        //    if (_context.Comments == null)
        //    {
        //        _context.Comments = _context.Set<Comment>();
        //    }

        //    Comments = await _context.Comments.Where(c => c.RecipeId == id).ToListAsync();

        //    if (Comments == null)
        //    {
        //        Comments = new List<Comment>();
        //    }

        //    return Page();
        //}

        //public async Task<IActionResult> OnPostAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    NewComment.UserId = userId; // UserId değerini ayarla

        //    NewComment.RecipeId = Recipe.Id;

        //    _context.Comments.Add(NewComment);
        //    await _context.SaveChangesAsync();

        //    Comments = await _context.Comments.Where(c => c.RecipeId == id).ToListAsync();
        //    NewComment = new Comment();

        //    return RedirectToPage("./Details", new { id = Recipe.Id });
        //}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            else
            {
                Recipe = recipe;
            }

            if (_context.Comments == null)
            {
                _context.Comments = _context.Set<Comment>();
            }

            Comments = await _context.Comments
                .Where(c => c.RecipeId == id)
                .ToListAsync();

            if (Comments == null)
            {
                Comments = new List<Comment>();
            }

            foreach (var comment in Comments)
            {
                var user = await _userManager.FindByIdAsync(comment.UserId);
                comment.UserId = user?.Email ?? comment.UserId;
            }


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            NewComment = new Comment { UserId = userId };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Yorumları tekrar yüklemek için doğru sorguyu yapın
                Comments = await _context.Comments.Where(c => c.RecipeId == id).ToListAsync();
                return Page();
            }

            
            NewComment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); ;
            NewComment.RecipeId = recipe.Id;
            NewComment.Content = Request.Form["NewComment.Content"];
            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            // Yorumları tekrar yüklemek için doğru sorguyu yapın
            Comments = await _context.Comments.Where(c => c.RecipeId == id).ToListAsync();

            // Yeni bir yorum nesnesi oluştur
            NewComment = new Comment();

            return RedirectToPage("./Details", new { id = recipe.Id });
        }
    }
}
