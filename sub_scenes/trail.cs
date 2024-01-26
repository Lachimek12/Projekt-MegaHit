using Godot;
using System;
using System.Collections.Generic;

public partial class trail : Line2D
{
	//////////////////////////////////////////////////////
	///////                 DATA                   ///////
	//////////////////////////////////////////////////////


	private const int MaxLength = 10;
	private Queue<Vector2> queue = new Queue<Vector2>();
	private Node2D parentNode;


	//////////////////////////////////////////////////////
	///////              FUNCTIONS                 ///////
	//////////////////////////////////////////////////////


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		parentNode = (Node2D)GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = Vector2.Zero;
		GlobalRotation = 0;

		Vector2 position = parentNode.GlobalPosition;
		queue.Enqueue(position);

		if (queue.Count > MaxLength)
		{
			queue.Dequeue();
		}

		ClearPoints();

		foreach (Vector2 pos in queue)
		{
			AddPoint(pos);
		}
	}
}
