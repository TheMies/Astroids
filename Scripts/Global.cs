using Godot;
using GC = Godot.Collections;
using System;

public class Global : Node{

    // Game settings
    public bool GameOver = false;
    public int Score = 0;
    public int Level = 0;

    // Player settings
    public double ShieldMax = 100;
    public double ShieldRegen = 10;
    
    
    // Astroid settings
    public GC.Dictionary<string, string> BreakPattern = new GC.Dictionary<string, string>{
        {"big", "med"},
        {"med", "small"},
        {"small", "tiny"},
        {"tiny", null},
    };
    public GC.Dictionary<string, int> AstroidDamage = new GC.Dictionary<string, int>{
        {"big", 40},
        {"med", 25},
        {"small", 15},
        {"tiny", 10},
    };
    public GC.Dictionary<string, int> AstroidPoints = new GC.Dictionary<string, int>{
        {"big", 10},
        {"med", 15},
        {"small", 25},
        {"tiny", 40},
    };
}