using Godot;

public partial class Money : Label
{
    public override void _Ready()
    {
        // Connect to GameManager's ScoreChanged signal
        var score = GetNode<ScoreKeeper>("/root/ScoreKeeper");
        score.ScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int money)
    {
        Text = $"Money: {money}$";
    }
}