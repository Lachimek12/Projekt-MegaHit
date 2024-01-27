using Godot;
using System;

public partial class projectile : Area2D
{
	//////////////////////////////////////////////////////
	///////                 DATA                   ///////
	//////////////////////////////////////////////////////


	private const float Speed = 800.0f;
	public float direction = 1;
	private double timePassedToDestroy = 0;
	private bool outOfWindow = false;

	// Get particle asset
	[Export]
	public PackedScene Particle;


	//////////////////////////////////////////////////////
	///////              FUNCTIONS                 ///////
	//////////////////////////////////////////////////////


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Bullets");
	}

	// Called every fixed time, for physics processes
    public override void _PhysicsProcess(double delta)
    {
		Vector2 velocity = Vector2.Zero;
		velocity.X = Speed * (float)delta * direction;
		Translate(velocity);

		// Thanks to that the trail behind the bullet can also exit the window screen and not just disappear
		if (outOfWindow)
		{
			timePassedToDestroy += delta;
			if (timePassedToDestroy > 0.5)
			{
				QueueFree();
			}
		}
    }


	//////////////////////////////////////////////////////
	///////          CONNECTED FUNCTIONS           ///////
	//////////////////////////////////////////////////////


	public void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		outOfWindow = true;
	}

	public void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("TileMap"))
		{
			Node2D particle = (Node2D)Particle.Instantiate();
			GpuParticles2D particle2D = (GpuParticles2D)particle;
			particle.Position = GlobalPosition;
			particle.Rotation = GlobalRotation;
			particle2D.Emitting = true;
			GetTree().CurrentScene.AddChild(particle);

			QueueFree();
		}
	}
}
