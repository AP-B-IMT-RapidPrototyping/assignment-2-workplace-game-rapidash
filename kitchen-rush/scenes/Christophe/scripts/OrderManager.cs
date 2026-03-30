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

    public override void _Process(double delta)
    {
        float d = (float)delta;

        timer += d;
        gameTime += d;

        float currentInterval = GetCurrentInterval();

        if (timer >= currentInterval)
        {
            timer = 0f;
            TryAddOrder();
        }
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

        // basis burger
        order.RequiredIngredients.Add(IngredientType.BunBottom);
        order.RequiredIngredients.Add(IngredientType.Burger);

        // kies random aantal extras
        var extrasList = new List<IngredientType> { IngredientType.Sla, IngredientType.Tomaat };
        var rng = new Random();
        int extrasToAdd = rng.Next(0, extrasList.Count + 1); // 0, 1 of 2 extras

        // shuffle de extras en pak er extrasToAdd van
        for (int i = 0; i < extrasToAdd; i++)
        {
            int index = rng.Next(extrasList.Count);
            order.RequiredIngredients.Add(extrasList[index]);
            extrasList.RemoveAt(index); // zodat geen duplicates
        }

        // top bun altijd
        order.RequiredIngredients.Add(IngredientType.BunTop);

        return order;
    }

    public void UpdateUI()
    {
        string text = "Orders:\n\n";

        for (int i = 0; i < orders.Count; i++)
        {
            text += $"Order {i + 1}:\n";
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

        if (gameTime < 20f)
        {
            interval = 5f; // easy
        }
        else if (gameTime < 60f)
        {
            interval = 3f; // medium
        }
        else if (gameTime < 120f)
        {
            interval = 2f; // hard
        }
        else
        {
            interval = 1.2f; // VERY HARD
        }

        interval -= rep * 0.05f;

        interval = Mathf.Max(interval, 0.8f);

        GD.Print($"Interval: {interval:0.00} | Time: {gameTime:0}");

        return interval;
    }
    public float GetGameTime()
    {
        return gameTime;
    }
}