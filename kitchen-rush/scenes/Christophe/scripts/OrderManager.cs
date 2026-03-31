using Godot;
using System;
using System.Collections.Generic;

public partial class OrderManager : Node
{
    [Signal]
    public delegate void OrdersChangedEventHandler(string ordersText);

    [Export] public float OrderInterval = 5f;
    [Export] public int MaxOrders = 2;

    private List<OrderData> orders = new List<OrderData>();

    [Export] public float BaseInterval = 5f;
    [Export] public float MinInterval = 1.5f;

    [Export] public float TimeFactor = 0.02f; // hoe sneller over tijd
    [Export] public float RepFactor = 0.1f;   // hoe sneller door rep

    private float timer = 0f;
    private float gameTime = 0f;

    IngredientType[] extras = { IngredientType.Sla, IngredientType.Tomaat };

    [Export] public float OrderLifetime = 30f; // tijd voor order verdwijnt
    [Export] public int MaxFails = 20;
    private int failCount = 0;
    [Signal]
    public delegate void FailsChangedEventHandler(int fails);

    private bool tutorialActive = false;
    private bool tutorialCompleted = false;

    private int nextOrderId = 1;

    public override void _Process(double delta)
    {
        if (tutorialActive)
            return;

        float d = (float)delta;

        timer += d;
        gameTime += d;

        // BEGINFASE
        if (gameTime < 60f)
        {
            if (orders.Count == 0)
            {
                TryAddOrder();
            }

            CheckExpiredOrders();
            return; 
        }

        // NORMALE GAME
        float currentInterval = GetCurrentInterval();

        if (timer >= currentInterval)
        {
            timer = 0f;
            TryAddOrder();
        }

        CheckExpiredOrders();
    }

    private void TryAddOrder()
    {
        if (orders.Count >= MaxOrders)
            return;

        orders.Add(GenerateRandomOrder());
        UpdateUI();
    }

    private OrderData GenerateRandomOrder()
    {
        var order = new OrderData();

        order.Id = nextOrderId;
        nextOrderId++;

        // basis burger
        order.RequiredIngredients.Add(IngredientType.BunBottom);
        order.RequiredIngredients.Add(IngredientType.Burger); // basis patty

        var rng = new Random();

        // bepaal aantal extras
        int extraCount;

        if (gameTime < 30f)
            extraCount = 0;
        else if (gameTime < 60)
            extraCount = 1;
        else if (gameTime < 120f)
        {
            extraCount = 2;
            MaxOrders = 3;
        }
        else
            extraCount = 3;

        var extrasList = new List<IngredientType>
        {
            IngredientType.Sla,
            IngredientType.Tomaat,
        };

        // NA 2 MIN → extras + duplicates toegestaan
        var allExtrasList = new List<IngredientType>
        {
            IngredientType.Sla,
            IngredientType.Tomaat,
            IngredientType.Burger // duplicaten nu toegestaan
        };

        // VOOR 2 MIN → geen duplicates
        if (gameTime < 120f)
        {
            for (int i = 0; i < extraCount && extrasList.Count > 0; i++)
            {
                int index = rng.Next(extrasList.Count);
                order.RequiredIngredients.Add(extrasList[index]);
                extrasList.RemoveAt(index);
            }
        }
        // NA 2 MIN → duplicates toegestaan
        else
        {
            for (int i = 0; i < extraCount; i++)
            {
                int index = rng.Next(allExtrasList.Count);
                order.RequiredIngredients.Add(allExtrasList[index]);
            }
        }

        // top bun altijd
        order.RequiredIngredients.Add(IngredientType.BunTop);

        order.SpawnTime = gameTime;

        return order;
    }

    public void UpdateUI()
    {
        string text = "Orders:\n\n";

        for (int i = 0; i < orders.Count; i++)
        {
            text += $"Order #{orders[i].Id}:\n";            
            text += orders[i].GetDisplayText();
            text += "\n";
        }

        EmitSignal(SignalName.OrdersChanged, text);
    }

    public List<OrderData> GetOrders()
    {
        return orders;
    }

    private float GetCurrentInterval()
    {
        var repManager = GetNode<ReputationManager>("/root/ReputationManager");
        int rep = repManager.GetRep();

        float interval;

        if (gameTime < 80f)
        {
            interval = 10f; 
        }
        else if (gameTime < 120f)
        {
            interval = 6f;
        }
        else if (gameTime < 180f)
        {
            interval = 4f;
        }
        else
        {
            interval = 2f;
        }

        // 🔹 REP maakt game sneller maar minder agressief
        interval -= rep * 0.02f;

        // minimum cap
        interval = Mathf.Max(interval, 1.2f);

        return interval;
    }
    public float GetGameTime()
    {
        return gameTime;
    }
    public void StartTutorial()
    {
        tutorialActive = true;
        tutorialCompleted = false;

        orders.Clear();

        var tutorialOrder = new OrderData();

        tutorialOrder.RequiredIngredients.Add(IngredientType.BunBottom);
        tutorialOrder.RequiredIngredients.Add(IngredientType.Burger);
        tutorialOrder.RequiredIngredients.Add(IngredientType.Sla);
        tutorialOrder.RequiredIngredients.Add(IngredientType.Tomaat);
        tutorialOrder.RequiredIngredients.Add(IngredientType.BunTop);

        orders.Add(tutorialOrder);

        UpdateUI();

        GD.Print("📘 Tutorial gestart");
    }
    public void CompleteTutorial()
    {
        tutorialActive = false;
        tutorialCompleted = true;

        orders.Clear();
        UpdateUI();

        GD.Print("✅ Tutorial voltooid");
    }
    public bool IsTutorialActive()
    {
        return tutorialActive;
    }
    private void CheckExpiredOrders()
    {
        var expiredOrders = new List<OrderData>();

        foreach (var order in orders)
        {
            float timeAlive = gameTime - order.SpawnTime;

            if (timeAlive >= OrderLifetime)
            {
                expiredOrders.Add(order);
            }
        }

        foreach (var order in expiredOrders)
        {
            orders.Remove(order);
            failCount++;

            EmitSignal(SignalName.FailsChanged, failCount);

            GD.Print($"❌ Order verlopen! Fails: {failCount}/{MaxFails}");
        }

        if (expiredOrders.Count > 0)
            UpdateUI();

        // 💀 Game over check
        if (failCount >= MaxFails)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        GD.Print("💀 GAME OVER!");

        GetTree().Paused = true;

        // UI tonen
    }
}