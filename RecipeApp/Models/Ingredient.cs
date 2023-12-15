﻿using System;
using System.Collections.Generic;

namespace RecipeApp.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Quantitiy { get; set; }

    public string Unit { get; set; } = null!;

    public int RecipeId { get; set; }
}
