using Godot;
using System;

public partial class Hud : CanvasLayer
{

    [Export] public TextureRect vida1;
    [Export] public TextureRect vida2;
    [Export] public TextureRect vida3;
    int vida = 3;

    public void quitarVida()
    {
        if (vida <= 0) return;
        vida--;
        cambioVida();
    }

    private void cambioVida()
    {
        switch (vida)
        {
            case 0:
                vida1.Visible = false;
                vida2.Visible = false;
                vida3.Visible = false;
                break;
            case 1:
                vida1.Visible = true;
                vida2.Visible = false;
                vida3.Visible = false;
                break;
            case 2:
                vida1.Visible = true;
                vida2.Visible = true;
                vida3.Visible = false;
                break;
            case 3:
                vida1.Visible = true;
                vida2.Visible = true;
                vida3.Visible = true;
                break;
        }
    }

}
