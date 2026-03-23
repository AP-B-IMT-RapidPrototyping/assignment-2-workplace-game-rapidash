using Godot;
using System;

public partial class Stove : Node3D
{
    // 👉 Hier ga je later je burger scene in slepen
    [Export] public PackedScene BurgerScene;

    public void Interact(PlayerControl player)
    {
        GD.Print("🔥 INTERACT FUNCTION WORDT GEROEPEN");

        if (!player.HasObject())
        {
            if (BurgerScene != null)
            {
                GD.Print("🍔 Burger maken...");
                
                Node3D burger = BurgerScene.Instantiate<Node3D>();
                
                // Plaats de burger in de handen van de speler
                player.HoldObject(burger);
            }
            else
            {
                GD.Print("❌ BurgerScene is NULL! Sleep de asset in het BurgerScene veld.");
            }
        }
        else
        {
            GD.Print("❌ Je handen zijn vol!");
        }
    }
}