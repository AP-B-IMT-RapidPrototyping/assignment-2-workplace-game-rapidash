using Godot;
using System;

public partial class Order : Node3D
{
    [Export] public Plate targetPlate;

    [Export] public int sellPrice = 15; // prijs van volledige burger

    public void Interact(PlayerControl player)
    {
        GD.Print("🔔 Bestelling verwerken");

        if (targetPlate == null)
        {
            GD.Print("Geen plate gekoppeld!");
            return;
        }

        if (!targetPlate.HasIngredients())
        {
            GD.Print("Plate is leeg!");
            return;
        }

        // 🔹 kosten berekenen
        int cost = targetPlate.GetTotalCost();

        // 🔹 winst berekenen
        int profit = sellPrice - cost;

        GD.Print($"Kost: {cost} | Verkoop: {sellPrice} | Winst: {profit}");

        // 🔹 ScoreKeeper ophalen (autoload)
        var score = GetNode<ScoreKeeper>("/root/ScoreKeeper");

        // 🔹 geld aanpassen
        score.AddScore(profit);

        // 🔹 TODO: later -> gerecht naar klant sturen
        // player.HoldObject(targetPlate); // (nu nog niet nodig)

        // 🔹 plate leegmaken
        targetPlate.ResetPlate();

        GD.Print("Plate geleegd en geld verwerkt!");
    }
}