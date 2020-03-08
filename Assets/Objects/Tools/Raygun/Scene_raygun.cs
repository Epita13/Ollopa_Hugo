using Godot;
using System;

public class Scene_raygun : Node2D
{
    public override void _Ready()
    {
        if (GetTree().GetNodesInGroup("SceneRaygun").Count == 1) 
        	Connect("hit",(Node)GetTree().GetNodesInGroup("SceneRaygun")[0], "Show_hit" );
    }

    public void Show_hit(Vector2 hit_location)
    {
        //RayCast2D shoot_ray = GetNode<RayCast2D>("shoot_ray");
        Particles2D expl = GetNode<Particles2D>("Explosion");
        AddChild(expl);
        expl.Position=hit_location;
        expl.Emitting=true;
        RemoveChild(expl);
    }
}



