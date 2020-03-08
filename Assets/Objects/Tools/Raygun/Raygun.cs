using Godot;
using System;
using System.Reflection;

public class Raygun : Node2D
{

	[Signal]
	public delegate void hit(Vector2 xy);

	public override void _Ready()
    {
		AnimatedSprite ray = GetNode<AnimatedSprite>("Sprite_Ray");
		ray.Visible=false;
		//if (GetTree().GetNodesInGroup("SceneRaygun").Count == 1) 
		//	Connect("hit",(Node)GetTree().GetNodesInGroup("SceneRaygun")[0], "Show_hit" );
    }
	
	public void shoot()
	{
		RayCast2D shoot_ray = GetNode<RayCast2D>("shoot_ray");
		if (shoot_ray.IsColliding())
		{
			EmitSignal("hit", shoot_ray.GetCollisionPoint());
		}
	}

	private float count = 0f;
	

	public override void _PhysicsProcess(float delta)
    {
	    AnimatedSprite raygun = GetNode<AnimatedSprite>("Sprite_Raygun");
	    AnimatedSprite ray = GetNode<AnimatedSprite>("Sprite_Ray");
	    RayCast2D raycast = GetNode<RayCast2D>("shoot_ray");
	    
	    
	    raygun.FlipV = GetGlobalMousePosition().x < Position.x;
	    LookAt(GetGlobalMousePosition());
	    
		
	    if (Input.IsActionPressed("shoot"))
	    {
		    ray.Visible = true;
			
			count += delta;
			if (count >0.1f)
			{
				count=0f;
				ray.FlipV=(!ray.FlipV);
			}
			shoot();
	    }
		
	    if (Input.IsActionJustReleased("shoot"))
	    {
		    ray.Visible = false;
	    }

	    
    }
}

