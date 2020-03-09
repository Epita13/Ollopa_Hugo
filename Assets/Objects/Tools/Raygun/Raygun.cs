using Godot;
using System;
using System.Reflection;

public class Raygun : Node2D
{

	[Signal]
	public delegate void hit(Vector2 xy,Vector2 az);

	[Signal]
	public delegate void shooting(Vector2 xy, Vector2 az);
	
	

	public override void _Ready()
    {
	    Connect("shooting",(Node)GetTree().GetNodesInGroup("SceneRaygun")[0], "Shooting" );
	    Connect("hit",(Node)GetTree().GetNodesInGroup("SceneRaygun")[0], "Show_hit" );
    }
	
	public void shoot()
	{
		RayCast2D shoot_ray = GetNode<RayCast2D>("shoot_ray");
		if (shoot_ray.IsColliding())
		{
			EmitSignal("hit", Position,shoot_ray.GetCollisionPoint());
		}
		else
		{
			EmitSignal("shooting",Position,GetGlobalMousePosition());
		}
	}
	
	

	public override void _PhysicsProcess(float delta)
    {
	    AnimatedSprite raygun = GetNode<AnimatedSprite>("Sprite_Raygun");
	    
	    raygun.FlipV = GetGlobalMousePosition().x < Position.x;
	    LookAt(GetGlobalMousePosition());
	    
	    if (Input.IsActionJustPressed("shoot"))
	    {
		    AudioStreamPlayer audio = GetNode<AudioStreamPlayer>("Shooting_Sound");
		    audio.Playing = true;
	    }
	    
	    if (Input.IsActionPressed("shoot"))
	    {
		    shoot();
	    }
	    
    }
}

