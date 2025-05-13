using Godot;
using System;

public partial class CambioNivel2 : Area2D
{
    public override void _Ready()
    {
        this.BodyEntered += _on_cambio_nivel_2_body_entered;
    }
    private void _on_cambio_nivel_2_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            GetTree().ChangeSceneToFile("res://Assets/Scenes/Lvl3.tscn");
        }
    }
}