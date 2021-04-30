using Godot;
using System;
using SC = System.Collections.Generic;
using GC = Godot.Collections;


public class Astroid : KinematicBody2D
{
    [Export]
    private float Bounce = 1.1f;
    private Vector2 Vel = new Vector2();
    private float Rot = 0;
    private Vector2 ScreenSize;
    private Vector2 Extends;

    private Particles2D Puff;

    private string ArtPath = "res://art/";
    private GC.Dictionary<string, GC.Array<string>> Textures = new GC.Dictionary<string, GC.Array<string>>();
    private RandomNumberGenerator rnd = new RandomNumberGenerator();
    public override void _Ready()
    {
        Textures.Add("big", new GC.Array<string>{
            "sheet.meteorGrey_big1.atlastex",
            "sheet.meteorGrey_big2.atlastex",
            "sheet.meteorGrey_big3.atlastex",
            "sheet.meteorGrey_big4.atlastex",
            });
        Textures.Add("med", new GC.Array<string>{
            "sheet.meteorGrey_med1.atlastex",
            "sheet.meteorGrey_med2.atlastex",
            });
        Textures.Add("small", new GC.Array<string>{
            "sheet.meteorGrey_small1.atlastex",
            "sheet.meteorGrey_small2.atlastex",
            });
        Textures.Add("tiny", new GC.Array<string>{
            "sheet.meteorGrey_tiny1.atlastex",
            "sheet.meteorGrey_tiny2.atlastex",
            });

        rnd.Randomize();
        Vel = new Vector2(rnd.RandfRange(30, 100), 0 ).Rotated(rnd.RandfRange(0, (float)(2 * Math.PI)));
        Rot = rnd.RandfRange(-1.5f, 1.5f);
        ScreenSize = GetViewportRect().Size;
        Puff = GetNode<Particles2D>("Puff");
    }

    public void Init(string size, Vector2 pos){
        // Set texture
        var textureIndex = (int)rnd.RandiRange(0, 100) % Textures[size].Count;
        var texture = ResourceLoader.Load(ArtPath + Textures[size][textureIndex]) as Texture;
        GetNode<Sprite>("Sprite").Texture = texture;

        // Set collision
        var shape = new CircleShape2D();
        shape.Radius = Math.Min(texture.GetWidth()/2, texture.GetHeight()/2);
        var col = new CollisionShape2D();
        col.Shape = shape;
        AddChild(col);

        Extends = texture.GetSize() /2;
        Position = pos;
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
