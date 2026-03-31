using Godot;
using System;

public partial class HUD : Control
{
    private Label _timerLabel;
    private float _timePassed = 0f;

    public override void _Ready()
    {
        _timerLabel = GetNode<Label>("Timer");
    }

    public override void _Process(double delta)
    {
        _timePassed += (float)delta;

        int totalSeconds = (int)_timePassed;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        _timerLabel.Text = $"{minutes:00}:{seconds:00}";
    }
}