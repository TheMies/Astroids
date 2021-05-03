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
        Global = GetNode<Global>("/root/Global");
    }

    public void Shield(double value){
        ShieldNode.Value = value;
    }
    public void Score(int value){
        LabelNode.Text = value.ToString();
    }

    private Global Global;
    
	private void Update(Player player){
		var color = "green";
		if (player.ShieldLevel < 40){
			color = "red";
		}
		else if (player.ShieldLevel < 70){
			color = "yellow";
		}
		var texture = ResourceLoader.Load<Texture>($"res://Art/gui/barHorizontal_{color}_mid 200.png");
		ShieldNode.TextureProgress_ = texture;
        ShieldNode.Value = player.ShieldLevel;
        LabelNode.Text = Global.Score.ToString();
	}
   
   public void ShowMessage(string text){
       GetNode<Label>("Message").Text = text;
       GetNode<Label>("Message").Show();
       GetNode<Timer>("MessageTimer").Start();
   }
    public void OnMessageTimerTimeout(){
        GetNode<Label>("Message").Hide();
        GetNode<Label>("Message").Text = string.Empty;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("pause_toggle")){
            Global.IsPaused = !Global.IsPaused;
            GetTree().Paused = Global.IsPaused;
            if (Global.IsPaused){ 
                GetNode<Panel>("PausePopup").Show();
                GetNode<Label>("Message").Hide();
            }
            else{
                GetNode<Panel>("PausePopup").Hide();
                GetNode<Label>("Message").Show();
            }
        }
    }

}
