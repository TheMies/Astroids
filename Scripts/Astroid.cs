using Godot;
using System;

public class Astroid : KinematicBody2D
{
    [Export]
    private float Bounce = 1.1f;
    private Vector2 Vel = new Vector2();
    private float Rot = 0;
    private Vector2 ScreenSize;
    private Vector2 Extends;

    private Particles2D Puff;

    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        Vel = new Vector2(rng.RandfRange(30, 100), 0 ).Rotated(rng.RandfRange(0, (float)(2 * Math.PI)));
        Rot = rng.RandfRange(-1.5f, 1.5f);
        ScreenSize = GetViewportRect().Size;
        Extends = GetNode<Sprite>("Sprite").Texture.GetSize() /2;
        Puff = GetNode<Particles2D>("Puff");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Rotation += Rot * delta;
        var col = MoveAndCollide(Vel * delta);
        
        if (col != null){
            Vel = Vel.Bounce(col.Normal);
            Puff.GlobalPosition = col.Position;
            Puff.Emitting = true;
        }

        var pos = Position;
        if (pos.x > ScreenSize.x + Extends.x){
            pos.x = -Extends.x;
        }
        if (pos.x < -Extends.x){
            pos.x = ScreenSize.x + Extends.x;
        }
        if (pos.y > ScreenSize.y + Extends.y){
            pos.y = -Extends.y;
        }
        if (pos.y < -Extends.y){
            pos.y = ScreenSize.y + Extends.y;
        }

        Position = pos;
    }
}
