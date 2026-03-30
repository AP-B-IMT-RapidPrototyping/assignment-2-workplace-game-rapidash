using Godot;
using System;

public partial class Interaction : Node
{
    OrderManager orderManager;
    // orderManager.StartTutorial();

    public Interaction() {
        orderManager = GetNode<OrderManager>("/root/OrderManager");
    }
}
