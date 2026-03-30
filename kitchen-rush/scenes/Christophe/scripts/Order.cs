using Godot;
using System;

public partial class Order : Node3D
{
    [Export] public Plate targetPlate;
    [Export] public int basePoints = 10; // basis score voor correcte bestelling

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
        var repManager = GetNode<ReputationManager>("/root/ReputationManager");

        bool matched = false;

        foreach (var orderData in orders)
        {
            // 🔹 Correcte bestelling check
            if (targetPlate.MatchesOrder(orderData, failOnExtraIngredients: false))
            {
                GD.Print("Correcte bestelling!");

                int rep = repManager.GetRep();
                float multiplier = GetScoreMultiplier(rep);
                int finalPoints = Mathf.RoundToInt(basePoints * multiplier);

                GD.Print($"REP: {rep} | Score multiplier: x{multiplier}");
                score.AddScore(finalPoints);

                // REP gain gebaseerd op tijd
                float currentTime = orderManager.GetGameTime();
                float timeTaken = currentTime - orderData.SpawnTime;
                int repGain = timeTaken < 5f ? 3 : timeTaken < 10f ? 2 : 1;
                repManager.AddRep(repGain);
                GD.Print($"REP +{repGain} | Tijd: {timeTaken:0.0}s");

                // Verwijder order uit lijst en update UI
                orders.Remove(orderData);
                orderManager.UpdateUI();

                matched = true;
                break;
            }
        }

        // 🔹 Foute bestelling
        if (!matched)
        {
            GD.Print("Foute bestelling!");
            // REP gaat omlaag
            repManager.AddRep(-1);
            GD.Print("REP -1 voor foute bestelling");
        }

        // Plate altijd resetten
        targetPlate.ResetPlate();
    }

    private float GetScoreMultiplier(int rep)
    {
        if (rep < 5) return 1f;
        if (rep < 10) return 1.5f;
        if (rep < 20) return 2f;
        return 3f;
    }
}