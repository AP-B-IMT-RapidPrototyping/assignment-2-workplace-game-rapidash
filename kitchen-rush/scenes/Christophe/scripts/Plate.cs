using Godot;
using System;
using System.Collections.Generic;

public partial class Plate : Node3D
{
    private List<Ingredient> ingredients = new List<Ingredient>();

    [Export] public float stackHeight = 0.2f;

    public void PlaceIngredient(Node3D ingredientNode)
    {
        if (!(ingredientNode is Ingredient ingredient))
        {
            GD.Print("Node is geen ingredient: ", ingredientNode.Name);
            return;
        }

        float yOffset = 0f;
        foreach (var ing in ingredients)
            yOffset += ing.Height;

        ingredients.Add(ingredient);
        AddChild(ingredient);

        ingredient.Position = new Vector3(0, yOffset, 0);
        ingredient.Rotation = Vector3.Zero;

        GD.Print("Ingredient geplaatst op plate: ", ingredient.Name, " | Type: ", ingredient.Type);
    }

    public List<Ingredient> GetIngredients() => ingredients;

    public bool HasIngredients() => ingredients.Count > 0;

    public void ResetPlate()
    {
        foreach (var ing in ingredients)
        {
            if (IsInstanceValid(ing))
                ing.QueueFree();
        }
        ingredients.Clear();
        GD.Print("Plate is leeg gemaakt");
    }

    // 🔹 Verbeterde MatchesOrder
    // failOnExtraIngredients = false -> extra toppings zijn ok
    public bool MatchesOrder(OrderData order, bool failOnExtraIngredients = false)
    {
        List<IngredientType> required = new List<IngredientType>(order.RequiredIngredients);

        foreach (var ing in ingredients)
        {
            if (!required.Contains(ing.Type))
            {
                if (failOnExtraIngredients)
                    return false; // extra ingredient mag niet
            }
            else
            {
                required.Remove(ing.Type); // correct ingredient verwijderd uit lijst
            }
        }

        // check of alle vereiste ingredients aanwezig zijn
        return required.Count == 0;
    }
}