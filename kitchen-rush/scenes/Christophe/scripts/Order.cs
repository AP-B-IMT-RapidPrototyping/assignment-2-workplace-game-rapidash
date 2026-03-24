using Godot;
using System;

public partial class Bell : Node3D
{
    [Export] public Plate targetPlate;

    public void Interact(PlayerControl player)
    {
        GD.Print("🔔 Bell gebruikt");

        // speler moet lege handen hebben
        if (player.HasObject())
        {
            GD.Print("❌ Je handen zijn niet leeg!");
            return;
        }

        if (targetPlate == null)
        {
            GD.Print("❌ Geen plate gekoppeld!");
            return;
        }

        if (!targetPlate.HasIngredients())
        {
            GD.Print("❌ Plate is leeg!");
            return;
        }

        // geef de plate aan de speler
        player.HoldObject(targetPlate);

        // maak nieuwe lege plate (optioneel)
        targetPlate.ResetPlate();

        GD.Print("🍔 Plate opgepakt!");
    }
}