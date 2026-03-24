using Godot;

public partial class ScoreKeeper : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);

    private int _score = 100;

    public override void _Ready()
    {
        EmitSignal(SignalName.ScoreChanged, _score);
        //GD.Print("Start geld: ", _score);
    }

    public void AddScore(int points)
    {
        _score += points;
        //GD.Print($"Score: {_score}");
        EmitSignal(SignalName.ScoreChanged, _score);
    }
}