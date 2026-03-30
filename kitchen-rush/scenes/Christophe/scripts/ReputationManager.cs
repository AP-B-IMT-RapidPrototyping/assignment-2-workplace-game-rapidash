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

        // 🔹 clamp naar minimum 0
        _rep = Mathf.Max(_rep, 0);

        EmitSignal(SignalName.ReputationChanged, _rep);
    }
}