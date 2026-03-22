using Godot;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InvertIf

namespace TheCatacombsOfVelhart.Scripts.Base.Player;


// It will most likely be split into several nodes and rewrite later on, once the main structure is complete.
public partial class PlayerMovement : CharacterBody3D
{
	public const float GridSize = 1.0f; // Do not touch
	public const float MoveTime = 0.3f; // How much time is required for the player to move. Affects smoothness.
	public const float MoveDelay = 0.1f; // How much time between movements
	public const float RotateTime = 0.5f; // How much time we need to rotate player (camera)
	public const float BobberAmplitude = 0.01f; // Camera Shaking
	
	private Vector3 _startPosition; // Starting position for movement interpolation
	private Vector3 _targetPosition; // Target position for movement interpolation
	private float _moveTimer; // Timer for tracking movement progress
	private float _delayTimer; // Timer for tracking delay between moves
	private bool _isMoving; // Indicating if the player is currently moving
	private bool _isDelaying; // Indicating if the player is in delay state after moving
	private bool _isRotating; // Indicating if the player is currently rotating
	private float _rotateTimer; // Timer for tracking rotation progress
	private float _startRotation; // Starting rotation angle for rotation interpolation
	private float _targetRotation; // Target rotation angle for rotation interpolation
	private Rid _collisionRid; // Player rid (collision)
	
	public override void _Ready()
	{
		_targetPosition = GlobalPosition;
		_startPosition = GlobalPosition;
		_startRotation = GlobalRotation.Y;
		_targetRotation = GlobalRotation.Y;
		
		_collisionRid = GetNode<CollisionShape3D>("Collision").Shape.GetRid();
	}

	public override void _PhysicsProcess(double delta)
	{
		// If player current in move?
		if (_isMoving)
		{
			_moveTimer += (float)delta;
			float progress = Mathf.Clamp(_moveTimer / MoveTime, 0f, 1f);
			
			float easeProgress = 1f - Mathf.Pow(1f - progress, 2f);
			GlobalPosition = _startPosition.Lerp(_targetPosition, easeProgress);

			
			// camera bobble
			float bobble = Mathf.Sin(_moveTimer * 15f) * BobberAmplitude; // how strength is bobbled
			Camera3D camera = GetNode<Camera3D>("Camera3D");
			camera.GlobalRotation = new Vector3(bobble * 0.1f, camera.GlobalRotation.Y, bobble);

				if (progress >= 1.0f)
				{
					_isMoving = false;
					_isDelaying = true;
					_delayTimer = 0f;
					GlobalPosition = _targetPosition;
					camera.GlobalRotation = new Vector3(0, camera.GlobalRotation.Y, 0);
				}

			
		}
		// Wait for cooldown before allowing next movement
		else if (_isDelaying)
		{
			_delayTimer += (float)delta;
			if (_delayTimer >= MoveDelay)
			{
				_isDelaying = false;
			}
		}
		// Wait for cooldown before allowing next rotating
		else if (_isRotating)
		{
			_rotateTimer += (float)delta;
			float progress = Mathf.Clamp(_rotateTimer / RotateTime, 0f, 1f);
			
			float easeProgress = 1f - Mathf.Pow(1f - progress, 2f);
			float currentRotation = Mathf.Lerp(_startRotation, _targetRotation, easeProgress);
			
			GlobalRotation = new Vector3(GlobalRotation.X, currentRotation, GlobalRotation.Z);

			if (progress >= 1.0f)
			{
				_isRotating = false;
				GlobalRotation = new Vector3(GlobalRotation.X, _targetRotation, GlobalRotation.Z);
			}
		}
		else
		{
			HandleInput(); // Main input method. We accept inputs from  player only when they are completely stay.
		}
	}

	// Main input method for player
	private void HandleInput()
	{
		// camera rotation to left
		if (Input.IsActionJustPressed("camera_left"))  // Q
		{
			RotatePlayer(Mathf.Pi / 2); // +90 degrees
		}
		// camera rotation to right
		if (Input.IsActionJustPressed("camera_right")) // E 
		{
			RotatePlayer(-Mathf.Pi / 2); // -90 degrees
		}
		
		// movement input
		// Perhaps we should remove all directions but forward
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

		if (inputDir == Vector2.Zero) return;
		
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		direction = RoundToGridDirection(direction);

		Vector3 nextPosition = GlobalPosition + direction * GridSize;
		if (CanMoveTo(nextPosition))
		{
			_startPosition = GlobalPosition;
			_targetPosition = nextPosition;
			_isMoving = true;
			_moveTimer = 0f;
		}
	}

	// Technically, it is not the camera that rotates, but the player (along the Y-axis).
	private void RotatePlayer(float angle)
	{
		if (!_isRotating && !_isMoving && !_isDelaying)
		{
			_startRotation = GlobalRotation.Y;
			_targetRotation = _startRotation + angle;
			_isRotating = true;
			_rotateTimer = 0f;
		}
	}
	
	// Snap to cardinal direction
	private Vector3 RoundToGridDirection(Vector3 direction)
	{
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Z))
		{
			return direction.X > 0 ? Vector3.Right : Vector3.Left;
		}

		if (Mathf.Abs(direction.Z) > 0.1f)
		{
			return direction.Z > 0 ? Vector3.Back : Vector3.Forward;
		}

		return Vector3.Zero;
	}

	// works fine but need to change later (looks hacky)
	// make a sphere which participates in intersection. 0.3f is minimum for now
	// (The player will be able to pass through the wall).
	private bool CanMoveTo(Vector3 position)
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = new PhysicsShapeQueryParameters3D();
		query.Shape = new SphereShape3D { Radius = 0.3f };
		
		query.Exclude.Add(_collisionRid);
	
		var transform = Transform3D.Identity;
		transform.Origin = position;
		query.Transform = transform;
	
		var result = spaceState.IntersectShape(query);
		return result.Count == 0;
	}


	
	
}



