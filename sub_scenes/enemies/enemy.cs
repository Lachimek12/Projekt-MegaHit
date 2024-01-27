using Godot;
using System;

public partial class enemy : CharacterBody2D
{
	//////////////////////////////////////////////////////
	///////                 DATA                   ///////
	//////////////////////////////////////////////////////


	public const float Speed = 300.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    // Get particle asset
    [Export]
	public PackedScene Particle;


	//////////////////////////////////////////////////////
	///////              FUNCTIONS                 ///////
	//////////////////////////////////////////////////////


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Enemies");
	}

	/*public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		Velocity = velocity;
		MoveAndSlide();
	}*/

	//////////////////////////////////////////////////////
	///////          CONNECTED FUNCTIONS           ///////
	//////////////////////////////////////////////////////


	public void _on_area_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("Bullets"))
		{
			area.QueueFree();

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
