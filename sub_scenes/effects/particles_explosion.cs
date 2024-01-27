using Godot;
using System;

public partial class particles_explosion : GpuParticles2D
{
	private double timePassed = 0;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timePassed += delta;
		if (timePassed > 10)
		{
			QueueFree();
		}
	}
}
