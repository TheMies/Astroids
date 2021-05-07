using Godot;
using System;

public class Enemy : Area2D
{
    private PackedScene bulletScene = GD.Load<PackedScene>("res://Scenes/EnemyBullet.tscn");
    private Node bulletContainer;
    private Timer shootTimer;
    private Path2D path;
    private Node paths;
    private PathFollow2D follow;
    private Node2D remote;
    private int speed = 150;
    public Node2D target;

    public override void _Ready()
    {
        bulletContainer = GetNode<Node>("BulletContainer");
        shootTimer = GetNode<Timer>("ShootTimer");
        paths = GetNode<Node>("EnemyPaths");
        var rnd = new RandomNumberGenerator();
        rnd.Randomize();
        path = paths.GetChildren()[rnd.RandiRange(0, paths.GetChildCount() -1)] as Path2D;
        follow = new PathFollow2D();
        follow.Loop = false;
        path.AddChild(follow);
        remote = new Node2D();
        follow.AddChild(remote);
        shootTimer.WaitTime = 1.5f;
        shootTimer.Start();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        follow.Offset = follow.Offset + speed *delta;
        Position = remote.GlobalPosition;
    }

    public void OnVisibleScreenExited(){
        QueueFree();
    }

    public void OnShootTimerTimeout(){
        GetNode<AudioStreamPlayer2D>("Audio").Play();
        Shoot3();
    }

    private void Shoot1(){
        var dir = GlobalPosition - target.GlobalPosition;
        var b = bulletScene.Instance<EnemyBullet>();
        bulletContainer.AddChild(b);
        b.StartAt(dir.Angle() - (float)Math.PI/2, GlobalPosition);
    }
    private void Shoot3(){
        var dir = GlobalPosition - target.GlobalPosition;

        for (int i = -1; i <= 1 ;i++){
            var b = bulletScene.Instance<EnemyBullet>();
            bulletContainer.AddChild(b);
            b.StartAt(dir.Angle() - (float)Math.PI/2 + (i * 0.1f) , GlobalPosition);
        }
    }
}
