using Godot;
using System;

public partial class Protagonista : CharacterBody2D
{
    [Export] public float speed = 1;
    [Export] public AnimatedSprite2D sprite;
    [Export] public float Gravity = 1200f;
    [Export] public float JumpVelocity = -400f;

    private Vector2 velocity = Vector2.Zero;
    private bool atacando = false;

    public override void _PhysicsProcess(double delta)
    {
        // Aplicar gravedad si no estás en el suelo
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }
        else if (Input.IsActionJustPressed("salto"))
        {
            velocity.Y = JumpVelocity;
        }

        // Movimiento horizontal
        float direccion = Input.GetActionStrength("derecha") - Input.GetActionStrength("izquierda");
        velocity.X = direccion * speed;

        // Asignar la velocidad al CharacterBody2D
        Velocity = velocity;
        MoveAndSlide();
        velocity = Velocity; // Actualizar la velocidad después del movimiento

        // Animaciones
        if (!atacando)
        {
            if (direccion == 0)
            {
                sprite.Play("idle");
            }
            else if (direccion > 0)
            {
                sprite.FlipH = false;
                sprite.Play("correr");
            }
            else
            {
                sprite.FlipH = true;
                sprite.Play("correr");
            }
        }

        // Ataque
        if (Input.IsActionJustPressed("atacar") && !atacando)
        {
            sprite.Play("atacar");
            atacando = true;

            // Conectar la señal de animación final si no está conectada aún
            if (!sprite.IsConnected("animation_finished", new Callable(this, nameof(OnAnimationFinished))))
            {
                sprite.AnimationFinished += OnAnimationFinished;
            }
        }
    }

    private void OnAnimationFinished()
    {
        if (sprite.Animation == "atacar")
        {
            atacando = false;
        }
    }
}
