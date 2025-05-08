using Godot;
using System;

public partial class CambioNivel2 : Area2D
{
    private void _on_cambio_nivel_2_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            GetTree().ChangeSceneToFile("C:\\Users\\ismae\\Desktop\\TFC\\juego\\Assets\\Scenes\\Lvl3.tscn");
        }
    }
}