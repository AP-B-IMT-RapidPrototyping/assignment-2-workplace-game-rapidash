using Godot;
using System;
using System.Collections.Generic;

public partial class Plate : Node3D
{
    private List<Ingredient> ingredients = new List<Ingredient>(); // lijst van ingredients
    [Export] public float stackHeight = 0.2f;

    public void PlaceIngredient(Node3D ingredientNode)
    {
        // Zorg dat het ingredient een Ingredient script heeft
        if (!(ingredientNode is Ingredient ingredient))
        {
            GD.Print("❌ Node is geen ingredient: ", ingredientNode.Name);
            return;
        }

        ingredients.Add(ingredient);
        AddChild(ingredient);

        float yOffset = ingredients.Count * stackHeight;
        ingredient.Position = new Vector3(0, yOffset, 0);
        ingredient.Rotation = Vector3.Zero;

        GD.Print("Ingredient geplaatst op plate: ", ingredient.Name, " | Type: ", ingredient.Type);
    }

    public List<Ingredient> GetIngredients()
    {
        return ingredients;
    }

    // Optioneel: check of burger compleet is
    public bool IsBurgerComplete()
    {
        bool hasBunTop = false;
        bool hasBunBottom = false;
        bool hasPatty = false;

        foreach (var ing in ingredients)
        {
            switch (ing.Type)
            {
                case IngredientType.BunTop:
                    hasBunTop = true;
                    break;
                case IngredientType.BunBottom:
                    hasBunBottom = true;
                    break;
                case IngredientType.Burger:
                    hasPatty = true;
                    break;
            }
        }

        return hasBunTop && hasBunBottom && hasPatty;
    }

    public bool HasIngredients()
    {
        return ingredients.Count > 0;
    }

    public void ResetPlate()
    {
        // verwijder alle ingredient nodes uit de scene
        foreach (var ing in ingredients)
        {
            if (IsInstanceValid(ing))
            {
                ing.QueueFree();
            }
        }

        // maak lijst leeg
        ingredients.Clear();

        GD.Print("🧼 Plate is leeg gemaakt");
    }
}