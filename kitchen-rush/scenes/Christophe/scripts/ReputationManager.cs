using Godot;
using System;

public partial class ReputationManager : Node
{
    [Signal]
    public delegate void ReputationChangedEventHandler(int newRep);

    private int _rep = 0;

    public int GetRep() => _rep;

    public void AddRep(int amount)
    {
        _rep += amount;
        EmitSignal(SignalName.ReputationChanged, _rep);
        GD.Print("⭐ REP: ", _rep);
    }
}