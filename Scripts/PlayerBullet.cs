using Godot;
using System;

public class PlayerBullet : Area2D
{
    [Export]
    private Vector2 velocity = new Vector2();
    
    [Export]
    private float speed = 1000;
    private Global Global;
    public override void _Ready()
    {
        Global = GetNode<Global>("/root/Global");
    }

    private void OnPlayerBulletBodyEntered(Node body){
        if (body.GetGroups().Contains("astroids")){
            body.Call("Explode", velocity.Normalized());
            QueueFree();
        }
    }

    private void OnPlayerBulletAreaAntered(Area2D area2D){
        if (area2D.GetGroups().Contains("Enemies")){
            QueueFree();
            area2D.CallDeferred("Damage", Global.PlayerBulletDamage);
        }
    }

    public void StartAt(float dir, Vector2 pos){
        Rotation = dir;
        Position = pos;
        velocity = new Vector2(speed, 0).Rotated(dir - (float)(Math.PI / 2));
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Position += velocity * delta;
    }

    public void OnLifetime(){
        QueueFree();
    }

}
