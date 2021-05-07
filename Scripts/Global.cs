using Godot;
using GC = Godot.Collections;
using System;

public class Global : Node{

    // Game settings
    public bool IsPaused = false;
    public bool GameOver = false;
    public int Score = 0;
    public int Level = 0;
    public Node CurrentScene;
    public Node NewScene;

    // Player settings
    public double ShieldMax = 100;
    public double ShieldRegen = 2;
    
    
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

    public override void _Ready()
    {
        base._Ready();
        var root = GetTree().Root;
        CurrentScene = root.GetChild<Node>(root.GetChildCount() -1);
    }

    public void GotoScene(string path){
        var s = ResourceLoader.Load(path) as PackedScene;
        NewScene = s.Instance<Node>();
        GetTree().Root.AddChild(NewScene);
        GetTree().CurrentScene = NewScene;
        CurrentScene.QueueFree();
        CurrentScene = NewScene;
    }

    public void NewGame(){
        GameOver = false;
        Score = 0;
        Level = 0;
        GotoScene("res://Scenes/Main.tscn");
    }
}