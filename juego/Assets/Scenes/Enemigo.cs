using Godot;
using System;

public partial class Enemigo : CharacterBody2D
{
    [Export] public AnimatedSprite2D sprite;
    [Export] public float velocidad = 100.0f;
    [Export] public float distanciaPatrulla = 100.0f; // distancia que recorre antes de girar

    private Vector2 direccionPatrulla = Vector2.Right;
    private float distanciaRecorrida = 0.0f;

    public override void _PhysicsProcess(double delta)
    {
        // Calcular movimiento
        Vector2 movimiento = direccionPatrulla * velocidad * (float)delta;

        // Mover y acumular distancia
        MoveAndCollide(movimiento);
        distanciaRecorrida += movimiento.Length();

        // Cambiar de dirección si llegó al límite
        if (distanciaRecorrida >= distanciaPatrulla)
        {
            direccionPatrulla *= -1;
            distanciaRecorrida = 0.0f;
        }

        // Animación
        if (sprite != null)
        {
            sprite.Play("correr");
            sprite.FlipH = direccionPatrulla.X < 0;
        }
    }
}
