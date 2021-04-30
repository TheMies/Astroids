using Godot;
using GC = Godot.Collections;

public class Main : Node
{
	private GC.Dictionary<string, string> BreakPattern = new GC.Dictionary<string, string>();
 	private PackedScene astroidScene = GD.Load("res://Scenes/Astroid.tscn") as PackedScene;
	private Node astroidContainer;
	public override void _Ready()
	{
		BreakPattern.Add("big", "med");
		BreakPattern.Add("med", "small");
		BreakPattern.Add("small", "tiny");
		BreakPattern.Add("tiny", null);

		// Spawn some astroids
		SpawnAstroids(1);
		// var spawns = GetNode<Node>("SpawnLocations");
		// for (int i =0; i < 1; i++){
		// 	var spawn = spawns.GetChild(i) as Position2D;
		// 	SpawnAstroid("big", spawn.Position, Vector2.Zero);
		// }
	}

    public override void _Process(float delta)
    {
        base._Process(delta);
		if (astroidContainer != null && astroidContainer.GetChildCount() == 0){
			SpawnAstroids(2);
		}
    }

	private void SpawnAstroids(int number){
		var spawns = GetNode<Node>("SpawnLocations");
		for (int i =0; i < number; i++){
			var spawn = spawns.GetChild(i) as Position2D;
			SpawnAstroid("big", spawn.Position, Vector2.Zero);
		}
	}
    

	private void SpawnAstroid(string size, Vector2 pos, Vector2 vel){
		var astroid = this.astroidScene.Instance<Astroid>();
		astroidContainer = GetNode<Node>("AstroidContainer");
		astroidContainer.AddChild(astroid);
		astroid.CallDeferred("Init", size, pos, vel);
		astroid.OnExplode += OnExplode;
	}

	private void OnExplode(string size, Vector2 pos, Vector2 vel, Vector2 hitVel){
		var newSize = BreakPattern[size];
		if (newSize == null){return;}
		//for var offset 
		var newPos1 = pos + hitVel.Tangent().Clamped(25) * 1;
		var newPos2 = pos + hitVel.Tangent().Clamped(25) * -1;
		var newVel1 = vel + hitVel.Tangent() * 1;
		var newVel2 = vel + hitVel.Tangent() * -1;
		SpawnAstroid(newSize, newPos1, newVel1);
		SpawnAstroid(newSize, newPos2, newVel2);
	}
}
