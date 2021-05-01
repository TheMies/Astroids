using Godot;
using System;
using GC = Godot.Collections;


public class Astroid : KinematicBody2D
{
    [Signal]
    public delegate void ExpodeHandler(string size, Vector2 pos, Vector2 vel, Vector2 hitVelocity);
    public event ExpodeHandler OnExplode;

    [Export]
    private float MaxSpeed = 300f;
    [Export]
    private float Bounce = 1.1f;
    private Vector2 Vel = new Vector2();
    private float Rot = 0;
    private Vector2 ScreenSize;
    private Vector2 Extends;

    private Particles2D Puff;
    public string Size = "";

    private string ArtPath = "res://art/";
    private GC.Dictionary<string, GC.Array<string>> Textures = new GC.Dictionary<string, GC.Array<string>>();
    private RandomNumberGenerator rnd = new RandomNumberGenerator();
    
    public Astroid(){
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
    }

    public override void _Ready()
    {
        AddToGroup("astroids");
        rnd.Randomize();
        ScreenSize = GetViewportRect().Size;
        Puff = GetNode<Particles2D>("Puff");
    }

    public void Init(string initialSize, Vector2 initialPosition, Vector2 initialVelocity){
        Size = initialSize;
        if (initialVelocity.Length() > 0){
            Vel = initialVelocity;
        }
        else{
            Vel = new Vector2(rnd.RandfRange(30, 100), 0 ).Rotated(rnd.RandfRange(0, (float)(2 * Math.PI)));
        }
        Rot = rnd.RandfRange(-1.5f, 1.5f);

        // Set texture
        var textureIndex = (int)rnd.RandiRange(0, 100) % Textures[initialSize].Count;
        var texture = ResourceLoader.Load(ArtPath + Textures[initialSize][textureIndex]) as Texture;
        GetNode<Sprite>("Sprite").Texture = texture;

        // Set collision
        var shape = new CircleShape2D();
        shape.Radius = Math.Min(texture.GetWidth()/2, texture.GetHeight()/2);
        var col = new CollisionShape2D();
        col.Shape = shape;
        AddChild(col);

        Extends = texture.GetSize() /2;
        Position = initialPosition;
    }

    public void Explode(Vector2 hitVelocity){
        OnExplode?.Invoke(Size, Position, Vel, hitVelocity);
        QueueFree();
    }

    public override void _PhysicsProcess(float delta)
    {
        Vel = Vel.Clamped(MaxSpeed);
        base._PhysicsProcess(delta);
        Rotation += Rot * delta;
        var col = MoveAndCollide(Vel * delta);
        
        if (col != null){
            Vel = Vel.Bounce(col.Normal) * Bounce;
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
