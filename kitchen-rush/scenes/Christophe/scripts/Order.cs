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
        
        // check tutorial
        bool isTutorial = orderManager.IsTutorialActive();

        var orders = orderManager.GetOrders();
        var score = GetNode<ScoreKeeper>("/root/ScoreKeeper");
        var repManager = GetNode<ReputationManager>("/root/ReputationManager");

        foreach (var orderData in orders)
        {
            // 🔹 Correcte bestelling check
            if (targetPlate.MatchesOrder(orderData, false))
            {
                GD.Print("✅ Correcte bestelling!");

                if (!isTutorial)
                {
                    int rep = repManager.GetRep();
                    float multiplier = GetScoreMultiplier(rep);
                    int finalPoints = Mathf.RoundToInt(basePoints * multiplier);

                    score.AddScore(finalPoints);

                    float currentTime = orderManager.GetGameTime();
                    float timeTaken = currentTime - orderData.SpawnTime;

                    int repGain = timeTaken < 5f ? 3 : timeTaken < 10f ? 2 : 1;
                    repManager.AddRep(repGain);
                }
                else
                {
                    GD.Print("📘 Tutorial voltooid!");

                    orderManager.CompleteTutorial();
                }

                orders.Remove(orderData);
                orderManager.UpdateUI();

                break;
            }
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