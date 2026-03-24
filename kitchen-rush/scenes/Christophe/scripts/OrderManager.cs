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
    private float timer = 0f;

    public override void _Process(double delta)
    {
        timer += (float)delta;

        if (timer >= OrderInterval)
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

        // random extras
        var rng = new Random();

        if (rng.NextDouble() < 0.5)
            order.RequiredIngredients.Add(IngredientType.Kaas);

        if (rng.NextDouble() < 0.5)
            order.RequiredIngredients.Add(IngredientType.Sla);

        if (rng.NextDouble() < 0.5)
            order.RequiredIngredients.Add(IngredientType.Tomaat);

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
}