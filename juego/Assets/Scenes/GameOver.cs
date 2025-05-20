using Godot;
using System;

public partial class GameOver : CanvasLayer
{

    [Export] public Button botonReinicio;
    [Export] public Button botonSalir;
    public override void _Ready()
    {
        botonReinicio.Pressed += _on_button_pressed;
        botonSalir.Pressed += _on_button_salir_pressed;
    }

    public void mostrar()
    {
        Visible = true;
    }

    private void _on_button_pressed()
    {
        GetTree().ReloadCurrentScene();
    }

    private void _on_button_salir_pressed()
    {
        GetTree().Quit();
    }
}
