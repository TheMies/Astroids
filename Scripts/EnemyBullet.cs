using Godot;
using System;

public class EnemyBullet : Area2D
{
    private Global Global;
    private Vector2 vel = new Vector2();
    [Export(PropertyHint.None, "Speed")]
    private int speed = 150;
    public override void _Ready() { 
        Global = GetNode<Global>("/root/Global");
    }

    public void StartAt(float dir, Vector2 pos){
        Rotation = dir;
        Position = pos;
        vel = new Vector2(speed, 0).Rotated(dir - (float)Math.PI/2);
    }

    public override void _PhysicsProcess(float delta)
    {
        Position += vel * delta;
    }

    public void OnVisibleScreenExited(){
        QueueFree();
    }

    public void OnAareaEntered(Area2D area){
        if (area.GetGroups().Contains("Enemies")){
            return;
        }
        if (area.HasMethod("Damage")){
            QueueFree();
            area.CallDeferred("Damage", Global.EnemyBulletDamage);
        }
    }
}
