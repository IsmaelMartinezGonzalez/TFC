using Godot;
using System;

public partial class CambioNivel : Area2D
{
    private void _on_cambioNivel_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador")) // Asegúrate de que el jugador esté en este grupo
        {
            GetTree().ChangeSceneToFile("res://Assets/Scenes/Lvl2.tscn");
        }
    }
}
