using Godot;
using System;

public partial class Protagonista : CharacterBody2D
{
    [Export] public float speed = 200f;
    [Export] public AnimatedSprite2D sprite;
    [Export] public float Gravity = 1200f;
    [Export] public float JumpVelocity = -400f;

    [Export] public Area2D hitboxataque; // Área de ataque (padre de ataque_espada)
    [Export] public CollisionShape2D ataque_espada; // Forma de la hitbox
    [Export] public CollisionShape2D hurtbox; // Hitbox de recibir daño

    [Export] public float DashSpeed = 400f;
    [Export] public float DashDuration = 0.2f;
    [Export] public float DashCooldown = 1.0f; // Cooldown entre dashes por ahora de un segundo

    private Vector2 velocity = Vector2.Zero;
    private bool atacando = false;
    private int vida = 4;
    private bool dañado = false;
    private bool muerto = false;
    private bool dashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private int direccionDash = 1;

    public override void _Ready()
    {
        sprite.AnimationFinished += OnAnimationFinished;

        if (hitboxataque != null)
        {
            hitboxataque.Monitoring = false;
        }

        if (ataque_espada != null)
        {
            ataque_espada.Disabled = true;
        }

        hitboxataque.BodyEntered += _on_hitbox_ataque_body_entered;

    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaF = (float)delta;

        if (!muerto)
        {


            // Cooldown del dash
            dashCooldownTimer -= deltaF;

            // DASH
            if (dashing)
            {
                velocity.X = direccionDash * DashSpeed;
                velocity.Y = 0;
                dashTimer -= deltaF;
                hurtbox.Disabled = true;

                if (dashTimer <= 0)
                {
                    dashing = false;
                    hurtbox.Disabled = false;
                    animación();
                }

                Velocity = velocity;
                MoveAndSlide();
                return;
            }

            GD.Print(Position.Y);

            //Condicion de muerte por caida
            if (Position.Y > 700)
            {
                death();
            }

            // Gravedad y salto
            if (!IsOnFloor())
            {
                velocity.Y += Gravity * deltaF;
            }
            else if (Input.IsActionJustPressed("salto"))
            {
                velocity.Y = JumpVelocity;
            }

            // Movimiento horizontal
            float direccion = Input.GetActionStrength("derecha") - Input.GetActionStrength("izquierda");
            velocity.X = direccion * speed;

            Velocity = velocity;
            MoveAndSlide();
            velocity = Velocity;

            animación();

            // ATAQUE
            if (Input.IsActionJustPressed("atacar") && !atacando)
            {
                sprite.Play("atacar");
                atacando = true;

                if (hitboxataque != null)
                    hitboxataque.Monitoring = true;

                if (ataque_espada != null)
                    ataque_espada.Disabled = false;
            }

            // DASH (con cooldown)
            if (Input.IsActionJustPressed("dash") && !atacando && !dashing && dashCooldownTimer <= 0f)
            {
                dashing = true;
                dashTimer = DashDuration;
                dashCooldownTimer = DashCooldown; // Reiniciar cooldown
                direccionDash = sprite.Scale.X > 0 ? 1 : -1;
                sprite.Play("dash");
            }
        }
    }

    private void OnAnimationFinished()
    {
        if (sprite.Animation == "atacar")
        {
            atacando = false;

            if (hitboxataque != null)
                hitboxataque.Monitoring = false;

            if (ataque_espada != null)
                ataque_espada.Disabled = true;

            animación();
        }
        else if (sprite.Animation == "dash")
        {
            animación();
        }
    }

    private void animación()
    {
        if (atacando || dashing || muerto) return;

        float direccion = Input.GetActionStrength("derecha") - Input.GetActionStrength("izquierda");

        if (direccion > 0)
        {
            sprite.Scale = new Vector2(1, 1);
            hurtbox.Position = new Vector2(-25, hurtbox.Position.Y);
            ataque_espada.Position = new Vector2(5, ataque_espada.Position.Y);
        }
        else if (direccion < 0)
        {
            sprite.Scale = new Vector2(-1, 1);
            hurtbox.Position = new Vector2(-10, hurtbox.Position.Y);
            ataque_espada.Position = new Vector2(-40, ataque_espada.Position.Y);
        }

        if (!IsOnFloor())
        {
            if (velocity.Y < 0)
                sprite.Play("salto");
            else
                sprite.Play("caer");
        }
        else
        {
            if (direccion == 0)
                sprite.Play("idle");
            else
                sprite.Play("correr");
        }
    }

    private void _on_hitbox_ataque_body_entered(Node body)
    {
        if (body.IsInGroup("Enemigo"))
        {
            ((Skeleton)body).damaged();
        }
    }

    public async void damaged()
    {
        if (dañado || muerto) return;
        dañado = true;
        vida--;
        GD.Print(vida);
        if (vida <= 0)
        {
            death();
        }
        // sprite.Play("dañado");
        // await ToSignal(sprite, "animation_finished");
        dañado = false;
    }

    private async void death()
    {
        hurtbox.Disabled = true;
        muerto = true;
        sprite.Play("muerto");
        await ToSignal(sprite, "animation_finished");
        GetTree().ReloadCurrentScene();
    }
}
