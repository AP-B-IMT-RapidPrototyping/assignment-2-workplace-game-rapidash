using Godot;
using System;

public enum IngredientType
{
    BunTop,
    BunBottom,
    Burger,
    Kaas,
    Tomaat,
    Sla
}

public partial class Ingredient : Node3D
{
    [Export] public IngredientType Type;
}