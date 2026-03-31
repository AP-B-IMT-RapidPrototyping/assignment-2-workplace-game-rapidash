using Godot;
using System;

public partial class OrderPaper : Control
{
    private Label orderLabel;

    public override void _Ready()
    {
        orderLabel = GetNode<Label>("OrderLabel");
    }

    public void SetOrderText(string text)
    {
        orderLabel.Text = text;
    }
}