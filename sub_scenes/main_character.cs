using Godot;
using System;

public partial class main_character : CharacterBody2D
{
	public const float Speed = 400.0f;
	public const float JumpVelocity = -900.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    [Export]
	public AnimatedSprite2D characterSprite;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

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
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 100);
		}

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
