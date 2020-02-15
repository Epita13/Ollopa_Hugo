using Godot;
using System;
using System.Reflection;

public class Raygun : Node2D
{

	[Signal]
	public delegate void hit();

	public override void _Ready()
    {
		
    }
	
	public void shoot()
	{
		//Particles2D expl = GetNode<Particles2D>("Explosion");
		RayCast2D shoot_ray = GetNode<RayCast2D>("shoot_ray");
		if (shoot_ray.IsColliding())
		{
			/*expl.Position = (shoot_ray.GetCollisionPoint());
			expl.Emitting = true;*/
			EmitSignal("hit", shoot_ray.GetCollisionPoint());
		}
	}

	private float count = 0f;
	

	public override void _PhysicsProcess(float delta)
    {
	    
	    AnimatedSprite raygun = GetNode<AnimatedSprite>("Sprite_Raygun");
	    AnimatedSprite ray = GetNode<AnimatedSprite>("Sprite_Ray");
	    
	    
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

