using Godot;

public partial class RepLabel : Label
{
    public override void _Ready()
    {
        var repManager = GetNode<ReputationManager>("/root/ReputationManager");
        repManager.ReputationChanged += OnReputationChanged;
    }

    private void OnReputationChanged(int rep)
    {
        Text = $"REP: {rep}";
    }
}