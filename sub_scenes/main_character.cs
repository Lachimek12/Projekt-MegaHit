using Godot;
using System;

public partial class main_character : CharacterBody2D
{
	//////////////////////////////////////////////////////
	///////                 DATA                   ///////
	//////////////////////////////////////////////////////


	public const float Speed = 400.0f;
	public const float JumpVelocity = -900.0f;

	// Get projectile asset
	public PackedScene Projectile = GD.Load<PackedScene>("res://sub_scenes/projectile.tscn");

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    [Export] // Get sprite for handling animation states
	public AnimatedSprite2D characterSprite;


	//////////////////////////////////////////////////////
	///////              FUNCTIONS                 ///////
	//////////////////////////////////////////////////////


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Handle animation state
		if (velocity.X > 1 || velocity.X < -1)
		{
			characterSprite.Animation = "running";
		}
		else
		{
			characterSprite.Animation = "default";
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			characterSprite.Animation = "jumping";
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		//Handle shooting
		if (Input.IsActionJustPressed("shoot"))
		{
			Node2D bullet = (Node2D)Projectile.Instantiate();
			projectile projectileScript = bullet as projectile;
			GetParent().AddChild(bullet);
			bullet.Position = this.GlobalPosition;

			// Manage bullet direction
			if (characterSprite.FlipH == true)
			{
				bullet.Position += new Vector2(-30, -10);
				projectileScript.direction = -1;
			}
			else
			{
				bullet.Position += new Vector2(30, -10);
				projectileScript.direction = 1;
			}
		}

		// Get the input direction and handle the movement/deceleration.
		float direction = Input.GetAxis("left", "right");
		if (direction != 0)
		{
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 100);
		}

		// Set proper look direction
		if (velocity.X < 0)
		{
			characterSprite.FlipH = true;
		}
		else if (velocity.X > 0)
		{
			characterSprite.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
