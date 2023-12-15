using System;
using System.Collections.Generic;

namespace RecipeApp.Models;

public partial class Comment
{
    public int Id { get; set; }

    public string? Content { get; set; } = null!;

    public int RecipeId { get; set; }

    public string UserId { get; set; } = null!;
}
