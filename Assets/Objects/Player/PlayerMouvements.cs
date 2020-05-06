   using Godot;
using System;

public class PlayerMouvements : KinematicBody2D
{


	public static bool HasPlayer = false;
	
	public static PlayerMouvements instance;
	public static Vector2 size = new Vector2(1.625f,3);

	public static float GRAVITY = 10; 
	public static float SPEED = 125*2.5f*0.6f;
	public static float JUMP_POWER = -250*1.4f;
	public static bool canMove = true;

	Vector2 UP = new Vector2(0,-1);
	Vector2 vel;
	bool on_ground;
	
	private bool move_right = false;
	private bool move_left = false;
	private bool again = true;
	

	Vector2 depopos;
	public override void _Ready()
	{
		HasPlayer = true;
		depopos = Position;
		instance = this;
		if (!World.IsInit)
		{
			GD.Print("Player : Warning, le monde n'est pas initialisé");
		}
	}

	public static float GetX() => Convertion.Location2World(instance.Position).x;
	public static float GetY() => Convertion.Location2World(instance.Position).y;

	public static void Teleport(float x, float y)
	{
		instance.Position = Convertion.World2Location(new Vector2(x,y));
	}

	private void HorizontalMouvement()
	{
		AnimationPlayer bond = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite image = GetNode<Sprite>("Image");
		
		/////////Move Right
		
		if (Input.IsActionJustPressed("ui_right")&& move_left==false)
		{
			image.FlipH = false;
			if(IsOnFloor())
				bond.Play("Turn");
		}                                                        ///Cannot input Turn because it wont be seen and will be replace by run
		if (Input.IsActionPressed("ui_right") && move_left==false )
		{
			move_right = true;
			image.FlipH = false;
			vel.x = SPEED;
			if(IsOnFloor())
				bond.Play("Run");
		}
		else if (Input.IsActionJustReleased("ui_right") && move_left==false)
		{
			move_right = false;
			if(IsOnFloor())
				bond.Play("Turn_Back");
		}
		
		/////////Move Left
		if (Input.IsActionJustPressed("ui_left")&& move_right==false)
		{
			image.FlipH=true;
			if(IsOnFloor())
				bond.Play("Turn");
		}
		if (Input.IsActionPressed("ui_left") && move_right==false)
		{
			move_left = true;
			image.FlipH=true;
			vel.x = -SPEED;
			if(IsOnFloor())
				bond.Play("Run");
		}
		else if (Input.IsActionJustReleased("ui_left") && move_right==false)
		{
			move_left = false;
			if(IsOnFloor())
				bond.Play("Turn_Back");
		}
		
	}

	private void JUMP()                       /////////////// Jump
	{
		AnimationPlayer bond = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite image = GetNode<Sprite>("Image");
		
		if (IsOnFloor())
		{
			on_ground = true;
			vel.y = 0;
		}
		else
		{
			on_ground = false;
		}

		if (on_ground && Input.IsActionPressed("ui_up"))
		{
			bond.Play("Start_Jump");
			vel.y = JUMP_POWER;
			on_ground = false;
		}
		else if (vel.y != 0 && (!bond.IsPlaying() || bond.CurrentAnimation=="Run"))
		{
			bond.Play("Jump");
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		AnimationPlayer bond = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite image = GetNode<Sprite>("Image");
		
		vel.x = 0;
		
		if (Player.health <= 0 && again)   ///////// Death
		{
			bond.Play("Death");
			canMove = false;
			again = false;
		}
		
		if(canMove && (PlayerState.GetState()==PlayerState.State.Normal || PlayerState.GetState()==PlayerState.State.Build || PlayerState.GetState()==PlayerState.State.Link))
		{
			HorizontalMouvement();
			JUMP();
			if (vel.x == 0)      ////////// Idle
			{
				if (!bond.IsPlaying())
				{
					image.FlipH = false;
					bond.Play("Idle");
				}
			}
		}
		
		vel.y += GRAVITY;

		MoveAndSlide(vel,UP);
	}


  public override void _Process(float delta)
  {
		if (World.IsInit)
		{
			WorldEndTeleportation();
		}

		Player.RemoveEnergy(0.1f*delta);
		if(Player.energy==0)
		{
			canMove = false;
		}
		
			
  }



  private void WorldEndTeleportation()
  {
	  Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
	  int viewportSizeX = Mathf.FloorToInt(GetViewport().Size.x * CurrentCamera.GetXZoom());
	  Vector2 vecMin = Convertion.Location2World(p) * -1;
	  Vector2 vecMax = Convertion.Location2World(new Vector2(p.x*-1+viewportSizeX, p.y));
	  if (vecMin.x < 0)
	  {
		  int i = (int) Mathf.Abs(vecMin.x / Chunk.size);
		  for (int a = i; a >= 0; a--)
			World.GetChunkWithID(World.size-1-a).DrawClone(-16*(a+1));
	  }
	  if (vecMax.x >= World.size*Chunk.size)
	  {
		  int i = (int) Mathf.Abs((vecMax.x-World.size*Chunk.size) / Chunk.size);
		  for (int a = i; a >= 0; a--)
			  World.GetChunkWithID(a).DrawClone(World.size*Chunk.size+(a*Chunk.size));
	  }
	  if (GetX() < 0)
	  {
		  Teleport(World.size*Chunk.size + GetX(),GetY());
	  }
	  if (GetX() > World.size*Chunk.size)
	  {
		  Teleport(GetX()-World.size*Chunk.size,GetY());
	  }
  }
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  

  
}
