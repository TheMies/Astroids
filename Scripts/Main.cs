using Godot;
using System;

public class Main : Node
{
 	private PackedScene astroidScene = GD.Load("res://Scenes/Astroid.tscn") as PackedScene;
	public override void _Ready()
	{
		// Spawn some astroids
		var spawns = GetNode<Node>("SpawnLocations");
		for (int i =0; i < 5; i++){
			var astroid = this.astroidScene.Instance();
			AddChild(astroid);
			astroid.Call("Init", "big", spawns.GetChild(i));
		}
	}
}
