using Godot;
using System;

public partial class Order : Node3D
{
    [Export] public Plate targetPlate;
    [Export] public int sellPrice = 15;

    public void Interact(PlayerControl player)
    {
        GD.Print("Bestelling verwerken");

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

        var orderManager = GetNode<OrderManager>("/root/OrderManager");
        var orders = orderManager.GetOrders();

        var score = GetNode<ScoreKeeper>("/root/ScoreKeeper");

        int cost = targetPlate.GetTotalCost();
        bool foundMatch = false;

        foreach (var order in orders)
        {
            if (targetPlate.MatchesOrder(order))
            {
                //GD.Print("Correcte bestelling!");

                int profit = sellPrice - cost;

                GD.Print($"Kost: {cost} | Verkoop: {sellPrice} | Winst: {profit}");

                score.AddMoney(profit);

                orders.Remove(order);

                orderManager.UpdateUI();

                foundMatch = true;
                break;
            }
        }

        if (!foundMatch)
        {
            //GD.Print("Foute bestelling!");

            score.AddMoney(-cost);

            GD.Print($"Verlies: -{cost}");
        }

        targetPlate.ResetPlate();

        GD.Print("Plate geleegd!");
    }
}