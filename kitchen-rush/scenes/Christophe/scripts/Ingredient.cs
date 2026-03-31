using Godot;
using System;

public enum IngredientType
{
    BunTop,
    BunBottom,
    Burger,
    Sla,
    Tomaat
}

public partial class Ingredient : Node3D
{
    [Export] public IngredientType Type;
    [Export] public float Height = 0.2f;
}