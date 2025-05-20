using Godot;
using System;

public partial class WinScreen : CanvasLayer
{

    [Export] public Button botonReinicio;
    [Export] public Button botonSalir;


    public override void _Ready()
    {
        botonReinicio.Pressed += _on_button_pressed;
        botonSalir.Pressed += _on_button_salir_pressed;
    }

    private void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://Assets/Scenes/mundo.tscn");
    }

    private void _on_button_salir_pressed()
    {
        GetTree().Quit();
    }

}
