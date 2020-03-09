using Godot;
using System;

public class Trail : Particles2D
{
    private static float delay = 0.1f;
    private float t = 0;
    
    public override void _Ready()
    {
        
    }
    
    public override void _Process(float delta)
    {
        t += delta;
        if (t >= delay)
        {
            QueueFree();
        }
    }
}
