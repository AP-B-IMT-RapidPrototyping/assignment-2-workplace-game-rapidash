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

    [Export] public int Cost = 1; // kost van dit ingredient (instelbaar in inspector)
    [Export] public float Height = 0.2f;
}