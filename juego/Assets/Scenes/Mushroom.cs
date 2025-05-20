using Godot;
using System;
using System.Threading.Tasks;

public partial class Mushroom : CharacterBody2D
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
    private bool dañado = false;

    private bool jugadorDentro = true;
    private int vida = 4;

    private Vector2 direccionPatrulla = Vector2.Right;
    private float distanciaRecorrida = 0.0f;
    private Vector2 velocity = Vector2.Zero;

    private bool jugadorEnRango = false;

    public override void _Ready()
    {
        hitboxArea.BodyEntered += _on_hitbox_body_entered;
        hitboxArea.BodyExited += _on_hitbox_body_exited;
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaF = (float)delta;

        // Pausar comportamiento mientras ataca
        if (ocupado || muerto)
        {
            velocity = Vector2.Zero;
            Velocity = velocity;
            MoveAndSlide();
            return;
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
        // Moviemiento de la seta
        Vector2 movimiento = direccionPatrulla * velocidad;
        velocity.X = movimiento.X;
        distanciaRecorrida += Math.Abs(velocity.X * deltaF);

        //Si la distancia recorrida supero la distancia de patrulla, cambia la direccion y empieza de 0
        if (distanciaRecorrida >= distanciaPatrulla)
        {
            direccionPatrulla *= -1;
            distanciaRecorrida = 0.0f;
        }
        estado("correr", this);
        //Voltea el sprite de la seta
        sprite.FlipH = direccionPatrulla.X > 0;

        // Cambio de direccion de la hitbox de la seta
        if (direccionPatrulla.X < 0)
        {
            hitbox.Position = new Vector2(hurtbox.Position.X - 10, hitbox.Position.Y);
        }
        else
        {
            hitbox.Position = new Vector2(hurtbox.Position.X + 10, hitbox.Position.Y);
        }
        Velocity = velocity;
        MoveAndSlide();
    }

    private async void muerte()
    {
        estado("muerto", this);
    }
    public async void daño()
    {
        estado("dañado", this);
        if (vida <= 0)
        {
            muerte();
        }
    }
    private async void _on_hitbox_body_entered(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            if (!dañado)
            {
                jugadorDentro = true;
            }
            estado("atacar", body);
        }
    }
    private void _on_hitbox_body_exited(Node body)
    {
        if (body.IsInGroup("Jugador"))
        {
            jugadorDentro = false;
        }
    }
    private async void estado(string cmd, Node body)
    {
        switch (cmd)
        {
            case "atacar":
                if (muerto || ocupado) return;
                ocupado = true;
                velocity.X = 0;
                sprite.Play("atacar");
                await ToSignal(sprite, "animation_finished");
                if (jugadorDentro)
                {
                    ((Protagonista)body).daño();
                }
                ocupado = false;
                break;
            case "correr":
                if (ocupado || muerto) return;
                sprite.Play("correr");
                break;
            case "dañado":
                if (muerto) return;
                jugadorDentro = false;
                dañado = true;
                vida--;
                ocupado = true;
                sprite.Play("dañado");
                await ToSignal(sprite, "animation_finished");
                dañado = false;
                ocupado = false;
                break;
            case "muerto":
                muerto = true;
                hurtbox.Disabled = true;
                hitbox.Disabled = true;
                sprite.Play("muerto");
                await ToSignal(sprite, "animation_finished");
                QueueFree();
                break;
        }
    }
}
