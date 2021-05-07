using Godot;
using System;

public class EnemyBullet : Area2D
{
    private Vector2 vel = new Vector2();
    [Export(PropertyHint.None, "Speed")]
    private int speed = 350;
    public override void _Ready()
    {
        
    }

    public void StartAt(float dir, Vector2 pos){
        Rotation = dir;
        Position = pos;
        //velocity = new Vector2(speed, 0).Rotated(dir - (float)(Math.PI / 2));
        vel = new Vector2(speed, 0).Rotated(dir - (float)Math.PI/2);
    }

    public override void _PhysicsProcess(float delta)
    {
        Position += vel * delta;
    }

    public void OnVisibleScreenExited(){
        QueueFree();
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
