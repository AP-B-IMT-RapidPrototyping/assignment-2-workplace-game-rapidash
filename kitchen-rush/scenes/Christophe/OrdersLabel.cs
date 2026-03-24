using Godot;

public partial class OrdersLabel : Label
{
    public override void _Ready()
    {
        var manager = GetNode<OrderManager>("/root/OrderManager");
        manager.OrdersChanged += OnOrdersChanged;
    }

    private void OnOrdersChanged(string text)
    {
        Text = text;
    }
}