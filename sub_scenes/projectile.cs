using Godot;
using System;

public partial class projectile : Area2D
{
	//////////////////////////////////////////////////////
	///////                 DATA                   ///////
	//////////////////////////////////////////////////////


	private const float Speed = 800.0f;
	public float direction = 1;


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
    }


	//////////////////////////////////////////////////////
	///////          CONNECTED FUNCTIONS           ///////
	//////////////////////////////////////////////////////


	public void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}

	public void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("TileMap"))
		{
			QueueFree();
		}
	}
}
