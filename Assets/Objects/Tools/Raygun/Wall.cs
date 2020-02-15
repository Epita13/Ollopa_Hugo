using Godot;
using System;

public class Wall : Node2D
{
    private int wall_life = 10;
	
	public int Wall_life
	{
		get { return wall_life; }
		set
		{
			wall_life=value;
		}
	}

	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
