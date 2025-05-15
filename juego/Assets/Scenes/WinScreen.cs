using Godot;
using System;

public partial class WinScreen : CanvasLayer
{

    [Export] public Button botonReinicio;


    public override void _Ready()
    {
        botonReinicio.Pressed += _on_button_pressed;
    }

    private void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://Assets/Scenes/mundo.tscn");
    }

}
