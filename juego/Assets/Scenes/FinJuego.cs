using Godot;
using System;

public partial class FinJuego : Area2D
{

    [Export] public CanvasLayer winScreen;

    public override void _Ready()
    {
        this.BodyEntered += _on_fin_juego_body_entered;
    }
    private void _on_fin_juego_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            winScreen.Visible = true;
        }
    }

}
