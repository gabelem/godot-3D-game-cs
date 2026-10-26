using Godot;
using System;

public partial class Player : CharacterBody3D
{
	//class properties: movement speed and fall acceleration to represent gravity and character vel.
	[Export]
	public int Speed{ get; set; } = 14;
	
	[Export]
	public int FallAcceleration { get; set; } = 75;
	
	[Export]
	public int JumpImpulse { get; set; } = 20;
	
	[Export]
	public int BounceImpulse { get; set; } = 16;
	
	[Signal]
	public delegate void HitEventHandler();
	
	private void Die()
	{
		EmitSignal(SignalName.Hit);
		QueueFree();
	}
	
	private void OnMobDetectorBodyEntered(Node3D body)
	{
		GD.Print("Died");
		Die();
	}
	
	private Vector3 _targetVelocity = Vector3.Zero;
	
	//movement: input direction vecetor
	public override void _PhysicsProcess(double delta)
	{
		//local var to store input direction
		var direction = Vector3.Zero;
		
		//check each move input and update direction
		if(Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if(Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if(Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}
		if(Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}
		
			if(direction != Vector3.Zero)
			{
				direction = direction.Normalized();
				
				GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
			}
			
			if(direction != Vector3.Zero)
			{
				GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 4;
			} else
			{
				GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 1; 
			}
			
			_targetVelocity.X = direction.X * Speed;
			_targetVelocity.Z = direction.Z * Speed;
			
			if(!IsOnFloor())
			{
				_targetVelocity.Y -= FallAcceleration * (float)delta; // every frame our character falls
			}
			
			Velocity = _targetVelocity;
			MoveAndSlide();
			
			//Jump
			if(IsOnFloor() && Input.IsActionJustPressed("jump"))
			{
				_targetVelocity.Y = JumpImpulse;
			}
			
			for(int i = 0; i < GetSlideCollisionCount(); i++)
			{
				KinematicCollision3D playerCollision = GetSlideCollision(i);
				
				if(playerCollision.GetCollider() is Mob mob)
				{
					if(Vector3.Up.Dot(playerCollision.GetNormal()) > 0.1f)
					{
						mob.Squash();
						_targetVelocity.Y = BounceImpulse;
						break;
					}
				}
			}
			var pivot = GetNode<Node3D>("Pivot");
			pivot.Rotation = new Vector3(Mathf.Pi / 6.0f * Velocity.Y / JumpImpulse, pivot.Rotation.Y, pivot.Rotation.Z);
	}
}
