using Godot;
using System;

public partial class kitty : CharacterBody2D
{
	//////////////////////////////////////////////////////
	///////                 DATA                   ///////
	//////////////////////////////////////////////////////


	private const float Speed = 400.0f;
	private const float JumpVelocity = -900.0f;
	private float distanceToPlayerX = 0; // current distance between cat and the player
	private const float maxDistanceFromPlayer = 100; // distance from which cat starts to run towards player
	private const double waitBeforeGo = 0.5; // time to wait before going after player
	private double timeWaited = 0; // time already waited
	private Vector2 distanceToPlayer;
	private const float maxBeforeTeleport = 800; //distance before teleport cat to player (in case he went the other way :) )

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	[Export] // Get sprite for handling animation states
	public AnimatedSprite2D kittenSprite;
	// Get particle asset
	[Export]
	public PackedScene Particle;

	public Node2D player = null;

	private RandomNumberGenerator rng = new RandomNumberGenerator();


	//////////////////////////////////////////////////////
	///////              FUNCTIONS                 ///////
	//////////////////////////////////////////////////////


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (player != null)
		{
			distanceToPlayerX = player.GlobalPosition.X - GlobalPosition.X;

			// rescuing the cat
			distanceToPlayer = GlobalPosition;
			float realDistance = distanceToPlayer.DistanceTo(player.GlobalPosition);
			if (realDistance > maxBeforeTeleport)
			{
				GlobalPosition = player.GlobalPosition;

				Node2D particle = (Node2D)Particle.Instantiate();
				GpuParticles2D particle2D = (GpuParticles2D)particle;
				particle.Position = GlobalPosition;
				particle.Rotation = GlobalRotation;
				particle2D.Emitting = true;
				GetTree().CurrentScene.AddChild(particle);
			}
		}

		// Handle animation state
		if (velocity.X > 1 || velocity.X < -1)
		{
			kittenSprite.Animation = "running";
		}
		else
		{
			kittenSprite.Animation = "default";
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			kittenSprite.Animation = "jumping";
		}

		if ((distanceToPlayerX >= maxDistanceFromPlayer || distanceToPlayerX <= -maxDistanceFromPlayer))
		{
			// Handle Jump.
			if (player.GlobalPosition.Y < GlobalPosition.Y && IsOnFloor() && timeWaited >= waitBeforeGo)
			{
				velocity.Y = JumpVelocity;
			}

			if (timeWaited <= waitBeforeGo)
			{
				timeWaited += delta;
			}
		}
		else if (timeWaited > waitBeforeGo)
		{
			timeWaited = 0;
		}

		// handle the movement/deceleration.
		if (player != null)
		{
			if (distanceToPlayerX >= maxDistanceFromPlayer && timeWaited >= waitBeforeGo)
			{
				velocity.X = rng.RandfRange(Speed - 100, Speed + 100);
			}
			else if (distanceToPlayerX <= -maxDistanceFromPlayer && timeWaited >= waitBeforeGo)
			{
				velocity.X = -rng.RandfRange(Speed - 100, Speed + 100);
			}
			else if ((distanceToPlayerX < 20 && distanceToPlayerX > -20) || IsOnFloor())
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, 100);
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 100);
		}

		// Set proper look direction
		if (velocity.X < 0)
		{
			kittenSprite.FlipH = true;
		}
		else if (velocity.X > 0)
		{
			kittenSprite.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void _on_area_2d_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			player = body;
		}
	}
}
