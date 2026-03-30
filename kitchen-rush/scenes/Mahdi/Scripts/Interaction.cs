using Godot;
using System;

public partial class Interaction : Node
{
    var orderManager = GetNode<OrderManager>("/root/OrderManager");
    orderManager.StartTutorial();
}
