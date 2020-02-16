using Godot;
using System;

public class Scene_raygun : Node2D
{
    public override void _Ready()
    {
        KinematicBody2D raygun = GetNode<KinematicBody2D>("Raygun");
        raygun.Connect("hit",raygun, "show_hit" );
    }

    public void Raygun_hit(Vector2 hit_location)
    {
        //RayCast2D shoot_ray = GetNode<RayCast2D>("shoot_ray");
        Particles2D expl = GetNode<Particles2D>("Explosion");
        AddChild(expl);
        expl.SetPosition(hit_location);
        expl.SetEmitting(true);
    }
}



