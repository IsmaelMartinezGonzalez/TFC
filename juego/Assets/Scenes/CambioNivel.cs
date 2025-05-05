using Godot;
using System;

public partial class CambioNivel : Area2D
{
    private void _on_cambioNivel_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador")) // Asegúrate de que el jugador esté en este grupo
        {
            GetTree().ChangeSceneToFile("C:\\Users\\ismae\\Desktop\\TFC\\juego\\Assets\\Scenes\\Lvl2.tscn");
        }
    }
}
