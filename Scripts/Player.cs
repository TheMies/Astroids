using Godot;
using System;

public class Player : Area2D
{
    [Export(PropertyHint.None, "Rotation speed")]
    private float _rotationSpeed = 2.6f;

    [Export(PropertyHint.None, "Thrust")]
    private float _thrust = 500f;
    
    [Export(PropertyHint.None, "Max velocity")]
    private float _maxVelocity = 400f;

    [Export(PropertyHint.None, "Bullet")]
    private PackedScene bullet;
    [Export(PropertyHint.None, "Friction")]
    private float _friction = 0.65f;
    private Vector2 _screenSize = new Vector2();
    private Vector2 _velocity = new Vector2();
    private Vector2 _acceleration = new Vector2();
    private float rot = 0;
    private Vector2 pos = new Vector2();
    private Node bulletContainer;
    private Timer gunTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        pos = _screenSize/2;
        SetProcess(true);
        bulletContainer = GetNode("BulletContainer");
        gunTimer = GetNode<Timer>("GunTimer");
    }

    public override void _Process(float delta)
    {
        //base._Process(delta);
        if(Input.IsActionPressed("player_left")){
            rot -= (_rotationSpeed * delta);
        }
        if(Input.IsActionPressed("player_right")){
            rot += (_rotationSpeed * delta);
        }
        if(Input.IsActionPressed("player_thrust")){
            _acceleration = new Vector2(_thrust, 0).Rotated(rot);
        }
        else{
            _acceleration = new Vector2(0,0);
        }
        if(Input.IsActionPressed("player_shoot")){
            if (gunTimer.TimeLeft == 0){
                Shoot();
            }
        }
        
        _acceleration += _velocity * - _friction;
        _velocity += _acceleration * delta;
        pos += _velocity * delta;
        
        if (pos.x > _screenSize.x){
            pos.x = 0;
        }
        if (pos.x < 0){
            pos.x = _screenSize.x;
        }
        if (pos.y > _screenSize.y){
            pos.y = 0;
        }
        if (pos.y < 0){
            pos.y = _screenSize.y;
        }

        Position = pos;
        Rotation = rot + (float)(Math.PI / 2); // Since ship sprite was pointing up, fix with extra rotation of PI/2;
    }

    public void OnGunTimer(){}

    private void Shoot(){
        gunTimer.Start();
        var b = bullet.Instance() as PlayerBullet;
        bulletContainer.AddChild(b);
        b.StartAt(Rotation, GetNode<Position2D>("Muzzle").GlobalPosition);
    }
}
