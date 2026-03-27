using Godot;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InvertIf

namespace TheCatacombsOfVelhart.Scripts.Game.Player;


// It will most likely be split into several nodes and rewrite later on, once the main structure is complete.
public partial class PlayerMovement : CharacterBody3D
{
	public const float GridSize = 1.0f; // Do not touch
	public const float MoveTime = 0.3f; // How much time is required for the player to move. Affects smoothness.
	public const float MoveDelay = 0.1f; // How much time between movements
	public const float RotateTime = 0.5f; // How much time we need to rotate player (camera)
	public const float BobberAmplitude = 0.01f; // Camera Shaking
	
	//====MOVEMENT BLOCK START====
	private Vector3 _startPosition; // Starting position for movement interpolation
	private Vector3 _targetPosition; // Target position for movement interpolation
	private float _moveTimer; // Timer for tracking movement progress
	private float _delayTimer; // Timer for tracking delay between moves
	//====MOVEMENT BLOCK END======
	
	//====ROTATING BLOCK START====
	private float _rotateTimer;
	private float _startRotation;
	private float _targetRotation;
	//====ROTATING BLOCK END=====

	private enum PlayerMovementState {
		Moving,
		Rotating,
		Delaying,
		Idling
	}
	
	private PlayerMovementState _playerMovementState;
	
	private Rid _collisionRid; // Player rid (collision)
	
	private Camera3D _camera3D;
	private RayCast3D _rayCast3D;
	
	
	public override void _Ready()
	{
		_collisionRid = GetNode<CollisionShape3D>("Collision").Shape.GetRid();
		_camera3D = GetNode<Camera3D>("Camera3D");
		_rayCast3D = GetNode<RayCast3D>("RayCast");

		_playerMovementState = PlayerMovementState.Idling;
	}

	public override void _PhysicsProcess(double delta)
	{

		// If player currently in move?
		switch (_playerMovementState) {
			case PlayerMovementState.Moving: {
				// Increment movement timer by delta time
				_moveTimer += (float)delta;
				// Calculate movement progress from 0 to 1
				float progress = Mathf.Clamp(_moveTimer / MoveTime, 0f, 1f);

				// Apply ease-out quadratic function for smooth deceleration
				float easeProgress = 1f - Mathf.Pow(1f - progress, 2f);
				GlobalPosition = _startPosition.Lerp(_targetPosition, easeProgress);

				// Apply camera bobbing effect during movement (shaking)
				float bobble = Mathf.Sin(_moveTimer * 15f) * BobberAmplitude; // how strength is bobbled
				_camera3D.GlobalRotation = new Vector3(bobble * 0.1f, _camera3D.GlobalRotation.Y, bobble);

				// can't be more than 1 (because clamp), but it's float.
				// Check if movement animation is complete
				if (progress >= 1.0f) {
					// Stop movement and start input delay (cooldown between movement (see const))
					_playerMovementState = PlayerMovementState.Delaying;
					_delayTimer = 0f;
					// Set final position to avoid  errors
					GlobalPosition = _targetPosition;
					// Reset camera bobbing
					_camera3D.GlobalRotation = new Vector3(0, _camera3D.GlobalRotation.Y, 0);
				}

				break;
			}

			// Wait for cooldown before allowing next rotating
			case PlayerMovementState.Rotating: {
				// Increment the rotation timer by the elapsed time in seconds
				_rotateTimer += (float)delta;

				// Calculate animation progress from 0 to 1
				float progress = Mathf.Clamp(_rotateTimer / RotateTime, 0f, 1f);
				float easeProgress = 1f - Mathf.Pow(1f - progress, 2f);

				// Interpolate between the start and target rotation angles
				float currentRotation = Mathf.Lerp(_startRotation, _targetRotation, easeProgress);

				// rotate by Y axis
				GlobalRotation = new Vector3(GlobalRotation.X, currentRotation, GlobalRotation.Z);

				// Check if the animation has completed ( can't be more than 1f)
				if (progress >= 1.0f) {
					_playerMovementState = PlayerMovementState.Idling;

					// Target will be reached in any case
					GlobalRotation = new Vector3(GlobalRotation.X, _targetRotation, GlobalRotation.Z);
				}

				break;
			}

			// Wait for cooldown before allowing next movement
			case PlayerMovementState.Delaying: {
				_delayTimer += (float)delta;
				if (_delayTimer >= MoveDelay) {
					_playerMovementState = PlayerMovementState.Idling;
				}

				break;
			}
			case PlayerMovementState.Idling:
				HandleInput(); // Main input method. We accept inputs from  player only when they are completely stay.
				break;
		}
	}

	// Main input method for player
	private void HandleInput()
	{
		// camera rotation to left
		if (Input.IsActionPressed("camera_left"))  // Q
		{
			RotatePlayer(Mathf.Pi / 2); // +90 degrees
		}
		// camera rotation to right
		if (Input.IsActionPressed("camera_right")) // E 
		{
			RotatePlayer(-Mathf.Pi / 2); // -90 degrees
		}
		
		// movement input
		float forwardInput = Input.IsActionPressed("forward") ? -1.0f : 0.0f;

		// return if there is no input direction
		if (forwardInput == 0) return;

		// Convert input direction to world global space 
		Vector3 direction = (Transform.Basis * new Vector3(0, 0, forwardInput)).Normalized();


		Vector3 nextPosition = GlobalPosition + direction * GridSize;
		if (CanMoveToDirection())
		{
			// Store the current position as the starting point for the movement animation
			_startPosition = GlobalPosition;
			// Set the target position for the movement animation
			_targetPosition = nextPosition;
			// Enable the movement
			_playerMovementState =  PlayerMovementState.Moving;
			// Reset the movement timer
			_moveTimer = 0f;
		}
	}

	// Technically, it is not the camera that rotates, but the player (along the Y-axis).
	private void RotatePlayer(float angle)
	{
		if (_playerMovementState == PlayerMovementState.Idling)
		{
			_startRotation = GlobalRotation.Y;
			_targetRotation = _startRotation + angle;
			_playerMovementState = PlayerMovementState.Rotating;
			_rotateTimer = 0f;
		}
	}

	// just checks if player can move to position.
	private bool CanMoveToDirection()
	{
		return !_rayCast3D.IsColliding();
	}
	
}



