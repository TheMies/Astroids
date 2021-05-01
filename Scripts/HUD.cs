using Godot;
using System;

public class HUD : CanvasLayer
{
    private TextureProgress ShieldNode;
    private Label LabelNode;
    public override void _Ready()
    {
        LabelNode = GetNode<Label>("Score");
        ShieldNode = GetNode<TextureProgress>("Shield");
    }

    // public double Shield {get{
    //     return ShieldNode.Value;
    // }set{
    //     ShieldNode.Value = value;
    // }}
    // public int Score {get{
    //     return int.Parse(LabelNode.Text);
    // }set{
    //     LabelNode.Text = value.ToString();
    // }}

    public void Shield(double value){
        ShieldNode.Value = value;
    }
    public void Score(int value){
        LabelNode.Text = value.ToString();
    }
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
