using Godot;
using System;

public partial class MeshInstance3D2 : MeshInstance3D
{
	[Export]
	public MeshInstance3D sprite;
	[Export]
	public Area3D BG;
	// Game state
	private PhysicsDirectSpaceState3D spaceState;
	private Vector2 mousePos;
	private Vector3 pos;

	// Camera
	private Camera3D cam;

	// Raycast
	private Godot.Collections.Dictionary rayA;

	public override void _Process(double delta)
	{
		// Look at mouse position
		pos = ScreenPointToRay();
		pos.Y = 0;
		sprite.LookAt(pos, Vector3.Up, true);

		var mid = Position.Lerp(pos, 0.5f);
		cam.Position = new Vector3(mid.X, 70, mid.Z);
		BG.Position = new Vector3(Position.X, 30, Position.Z);
	}

	// Raycast
	public Vector3 ScreenPointToRay()
	{
		// Get the mouse position
		spaceState = GetWorld3D().DirectSpaceState;
		mousePos = GetViewport().GetMousePosition();
		// Get the camera
		cam = GetTree().Root.GetCamera3D();
		// Get the ray
		var rayO = cam.ProjectRayOrigin(mousePos);
		var rayE = rayO + cam.ProjectRayNormal(mousePos) * 2000;
		var query = PhysicsRayQueryParameters3D.Create(rayO, rayE);
		query.CollideWithAreas = true;
		// Get the raycast
		rayA = spaceState.IntersectRay(query);
		// Return the position of the raycast
		if (rayA != null)
			return (Vector3)rayA["position"];
		return new Vector3(0, 0, 0);
	}
}
