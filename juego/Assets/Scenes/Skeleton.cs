using Godot;
using System;
using System.Threading.Tasks;

public partial class Skeleton : CharacterBody2D
{
    [Export] public AnimatedSprite2D sprite;
    [Export] public float Gravity = 1200f;
    [Export] public CollisionShape2D hitbox;
    [Export] public Area2D hitboxArea;
    [Export] public CollisionShape2D hurtbox;
    [Export] public float velocidad = 100.0f;
    [Export] public float distanciaPatrulla = 200.0f;

    private bool muerto = false;
    private bool ocupado = false;

    private bool jugadorDentro = false;
    private int vida = 2;

    private Vector2 direccionPatrulla = Vector2.Right;
    private float distanciaRecorrida = 0.0f;
    private Vector2 velocity = Vector2.Zero;

    private bool jugadorEnRango = false;

    public override void _Ready()
    {
        if (hitbox != null)
        {
            hitbox.Disabled = true;
        }
        hitboxArea.BodyEntered += _on_hitbox_body_entered;
        hitboxArea.BodyEntered += _on_hitbox_body_exited;
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaF = (float)delta;

        if (muerto)
        {
            velocity = Vector2.Zero;
            Velocity = velocity;
            MoveAndSlide();
            if (sprite != null && sprite.Animation != "death")
                sprite.Play("death");
            return;
        }

        if (ocupado)
        {
            velocity = Vector2.Zero;
            Velocity = velocity;
            MoveAndSlide();
            return; // Pausar comportamiento mientras se daña
        }

        // Aplicar gravedad
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * deltaF;
        }
        else
        {
            velocity.Y = 0;
        }

        if (!jugadorEnRango)
        {
            // Patrulla
            Vector2 movimiento = direccionPatrulla * velocidad;
            velocity.X = movimiento.X;
            distanciaRecorrida += Math.Abs(velocity.X * deltaF);

            if (distanciaRecorrida >= distanciaPatrulla)
            {
                direccionPatrulla *= -1;
                distanciaRecorrida = 0.0f;
            }

            if (sprite != null)
            {
                sprite.Play("correr");
                sprite.FlipH = direccionPatrulla.X < 0;
            }

            // if (direccionPatrulla.X < 0)
            //     hitbox.Position = new Vector2( hitbox.Position.Y -5, hitbox.Position.Y);
            // else
            //     hitbox.Position = new Vector2( hitbox.Position.X + 5, hitbox.Position.Y);

            if (hitbox != null)
                hitbox.Disabled = false;
        }
        else
        {
            velocity.X = 0;

            if (sprite != null)
                sprite.Play("atacar");

            if (hitbox != null)
                hitbox.Disabled = false;
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private async void death()
    {
        if (muerto) return;
        muerto = true;
        hitbox.Disabled = true;
        hitboxArea.Monitoring = false;
        hurtbox.Disabled = true;
        sprite.Play("death");
        await ToSignal(sprite, "animation_finished");
        QueueFree();
    }

    public async void damaged()
    {
        if (ocupado || muerto) return; // Evita dañar si ya está en ese estado
        vida--;
        ocupado = true;

        if (vida <= 0)
        {
            death();
        }
        else
        {
            sprite.Play("damaged");
            await ToSignal(sprite, "animation_finished");
            ocupado = false; // Reanudar comportamiento
        }
    }

    private async void _on_hitbox_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            jugadorDentro = true;
            ocupado = true;
            sprite.Play("atacar");
            await ToSignal(sprite, "animation_finished");

            GD.Print(jugadorDentro);

            // if (jugadorDentro)
            // {
                ((Protagonista)body).damaged();
            // }

            ocupado = false;
        }
    }

    private void _on_hitbox_body_exited(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            GD.Print("chao");
            jugadorDentro = false;
        }
    }

}
