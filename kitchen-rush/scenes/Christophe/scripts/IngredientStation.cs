using Godot;
using System;

public partial class IngredientStation : Node3D
{
    // 👉 Hier ga je later je burger scene in slepen
    [Export] public PackedScene IngredientScene;

    public void Interact(PlayerControl player)
    {
        if (!player.HasObject())
        {
            if (IngredientScene != null)
            {                
                Node3D Ingredient = IngredientScene.Instantiate<Node3D>();
                
                // Plaats de burger in de handen van de speler
                player.HoldObject(Ingredient);
            }
            else
            {
                GD.Print("BurgerScene is NULL!");
            }
        }
        else
        {
            GD.Print("Je handen zijn vol!");
        }
    }
}