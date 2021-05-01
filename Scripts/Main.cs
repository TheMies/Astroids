using Godot;

public class Main : Node
{
 	private PackedScene astroidScene = GD.Load("res://Scenes/Astroid.tscn") as PackedScene;
 	private PackedScene explosionScene = GD.Load("res://Scenes/Explosion.tscn") as PackedScene;
	private Node astroidContainer;
    private AudioStreamPlayer explosion1;
	private Global Global;

	public override void _Ready()
	{
		Global = GetNode<Global>("/root/Global");
        explosion1 = GetNode<AudioStreamPlayer>("Explosion1");
	}

    public override void _Process(float delta)
    {
		ShowHudData();

        base._Process(delta);
		if (astroidContainer == null || astroidContainer.GetChildCount() == 0){
			Global.Level += 1;
			SpawnAstroids(Global.Level);
		}
    }

	private void ShowHudData(){
		var HUD = GetNode<CanvasLayer>("HUD");
		var player = GetNode<Player>("Player");
		var color = "green";
		if (player.ShieldLevel < 40){
			color = "red";
		}
		else if (player.ShieldLevel < 70){
			color = "yellow";
		}
		var texture = ResourceLoader.Load<Texture>($"res://Art/gui/barHorizontal_{color}_mid 200.png");
		HUD.GetNode<TextureProgress>("Shield").TextureProgress_ = texture;
		HUD.CallDeferred("Shield", player.ShieldLevel);
		HUD.CallDeferred("Score", Global.Score);
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
		explosion1.Play();
		Global.Score += Global.AstroidPoints[size];
		var newSize = GetNode<Global>("/root/Global").BreakPattern[size];
		if (newSize == null){
			ShowExplosion(false, pos);
			return;
		}
		
		//for var offset 
		var newPos1 = pos + hitVel.Tangent().Clamped(25) * 1;
		var newPos2 = pos + hitVel.Tangent().Clamped(25) * -1;
		var newVel1 = vel + hitVel.Tangent() * 1;
		var newVel2 = vel + hitVel.Tangent() * -1;
		SpawnAstroid(newSize, newPos1, newVel1);
		SpawnAstroid(newSize, newPos2, newVel2);

		ShowExplosion(true, pos);
	}

	private void ShowExplosion(bool big, Vector2 pos){
		var	expl = explosionScene.Instance() as AnimatedSprite;
		if (big){
			expl.Animation = "regular";
		}
		else{
			expl.Animation = "small";
		}
		AddChild(expl);
		expl.Position = pos;
		expl.Play();
	}
}
