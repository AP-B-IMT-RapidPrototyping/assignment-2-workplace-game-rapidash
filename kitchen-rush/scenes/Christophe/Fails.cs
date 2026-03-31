using Godot;

public partial class Fails : Label
{
    public override void _Ready()
    {
        var orderManager = GetNode<OrderManager>("/root/OrderManager");

        orderManager.FailsChanged += OnFailsChanged;

        // startwaarde tonen
        OnFailsChanged(0);
    }

    private void OnFailsChanged(int fails)
    {
        var orderManager = GetNode<OrderManager>("/root/OrderManager");

        Text = $"Fails: {fails}/{orderManager.MaxFails}";
    }
}