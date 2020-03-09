using Godot;
using System;

public class Explosion : Particles2D
{
    private static float delay = 0.3f;
    private float t = 0;
    
    public override void _Process(float delta)
    {
        t += delta;
        if (t >= delay)
        {
            QueueFree();
        }
    }
}
