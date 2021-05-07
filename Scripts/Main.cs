using Godot;

public class Main : Node
{
 	private PackedScene astroidScene = GD.Load("res://Scenes/Astroid.tscn") as PackedScene;
 	private PackedScene explosionScene = GD.Load("res://Scenes/Explosion.tscn") as PackedScene;
 	private PackedScene enemyScene = GD.Load("res://Scenes/Enemy.tscn") as PackedScene;
	private Node astroidContainer;
    private AudioStreamPlayer explosion1;
	private Global Global;
	private Timer EnemyTimer;
	private RandomNumberGenerator rnd = new RandomNumberGenerator();

	public override void _Ready()
	{
		rnd.Randomize();
		Global = GetNode<Global>("/root/Global");
        explosion1 = GetNode<AudioStreamPlayer>("Explosion1");
		EnemyTimer = GetNode<Timer>("EnemyTimer");
		GetNode<Player>("Player").OnExplode += OnExplodePlayer;
		BeginNextLevel();
	}

	private void OnExplodePlayer(){
		var player = GetNode<Player>("Player");
		player.CallDeferred("Disable");
		explosion1.Play();
		var	expl = explosionScene.Instance() as AnimatedSprite;
		expl.Animation = "sonic";
		expl.Scale = new Vector2(1.8f, 1.8f);
		AddChild(expl);
		expl.Position = player.Position;
		expl.Play();
		Global.GameOver = true;
		GetNode<HUD>("HUD").ShowMessage($"Game over!");
		GetNode<Timer>("RestartTimer").Start();
	}

	public void BeginNextLevel(){
		Global.Level += 1;
		EnemyTimer.Stop();
		EnemyTimer.WaitTime = rnd.RandiRange(5, 10);
		EnemyTimer.Start();
		GetNode<HUD>("HUD").ShowMessage($"Wave {Global.Level}");
		SpawnAstroids(Global.Level);
	}

    public override void _Process(float delta)
    {
		var HUD = GetNode<CanvasLayer>("HUD");
		HUD.CallDeferred("Update", GetNode<Player>("Player"));
        base._Process(delta);
		if (astroidContainer == null || astroidContainer.GetChildCount() == 0){
			BeginNextLevel();
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
		explosion1.Play();
		Global.Score += Global.AstroidPoints[size];
		var newSize = GetNode<Global>("/root/Global").BreakPattern[size];
		if (newSize == null){
			ShowExplosion(size, pos);
			return;
		}
		
		//for var offset 
		var newPos1 = pos + hitVel.Tangent().Clamped(25) * 1;
		var newPos2 = pos + hitVel.Tangent().Clamped(25) * -1;
		var newVel1 = vel + hitVel.Tangent() * 1;
		var newVel2 = vel + hitVel.Tangent() * -1;
		SpawnAstroid(newSize, newPos1, newVel1);
		SpawnAstroid(newSize, newPos2, newVel2);
		ShowExplosion(size, pos);
	}

	

	private void ShowExplosion(string size, Vector2 pos){
		var	expl = explosionScene.Instance() as AnimatedSprite;
		if (size == "big"){
			expl.Animation = "sonic";
		}
		else if (size == "med"){
			expl.Animation = "regular";
		}
		else if (size == "small"){
			expl.Animation = "small";
		}
		else if (size == "tiny"){
			expl.Animation = "small";
			expl.Scale = new Vector2(0.5f, 0.5f);
		}
		AddChild(expl);
		expl.Position = pos;
		expl.Play();
	}

	private void OnRestartTimerTimeout(){
		Global.NewGame();
	}

	private void OnEnemyTimerTimeout(){
		var e = enemyScene.Instance<Enemy>();
		AddChild(e);
		e.target = GetNode<Player>("Player");
		EnemyTimer.WaitTime = rnd.RandiRange(20, 40);
		EnemyTimer.Start();
	}
}
