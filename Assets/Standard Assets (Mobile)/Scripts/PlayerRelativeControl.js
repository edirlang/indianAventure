//////////////////////////////////////////////////////////////
// PlayerRelativeControl.js
// Penelope iPhone Tutorial
//
// PlayerRelativeControl creates a control scheme similar to what
// might be found in a 3rd person, over-the-shoulder game found on
// consoles. The left stick is used to move the character, and the
// right stick is used to rotate the character. A quick double-tap
// on the right joystick will make the character jump.
//////////////////////////////////////////////////////////////

#pragma strict

@script RequireComponent( CharacterController )

// This script must be attached to a GameObject that has a CharacterController
var moveJoystick : Joystick;
var rotateJoystick : Joystick;

var cameraPivot : Transform;						// The transform used for camera rotation

var forwardSpeed : float = 4;
var backwardSpeed : float = 1;
var sidestepSpeed : float = 1;
var jumpSpeed : float = 8;
var inAirMultiplier : float = 0.25;					// Limiter for ground speed while jumping
var rotationSpeed : Vector2 = Vector2( 50, 25 );	// Camera rotation speed for each axis

private var thisTransform : Transform;
private var character : CharacterController;
private var cameraVelocity : Vector3;
private var velocity : Vector3;	
private var nw : NetworkView; // Used for continuing momentum while in air
private var animator : Animator;
private var camara : GameObject;

function Start()
{
	nw = GetComponent(NetworkView);
	animator = GetComponent("Animator");
	camara = GameObject.Find( "Main Camera" );
	if ( camara )
		cameraPivot = camara.transform;	
	
	var joystick1 = GameObject.Find( "j1" );
	if ( joystick1 )
		moveJoystick = joystick1.GetComponent(Joystick);
		
	rotateJoystick = moveJoystick;
	// Cache component lookup at startup instead of doing this every frame		
	thisTransform = GetComponent( Transform );
	character = GetComponent( CharacterController );	

	// Move the character to the correct start position in the level, if one exists
	var spawn = GameObject.Find (Network.player.ipAddress);
	if ( spawn )
		thisTransform.position = spawn.transform.position;
}

function OnEndGame()
{
	// Disable joystick when the game ends	
	moveJoystick.Disable();
	rotateJoystick.Disable();	

	// Don't allow any more control changes when the game ends
	this.enabled = false;
}

function Update()
{
	if (nw.isMine)
	{
		camara.transform.parent = transform;
		camara.transform.localPosition = new Vector3(0.1300829f, 4.42427f, -7.354623f);
			
		var movement = thisTransform.TransformDirection( Vector3( moveJoystick.position.x, 0, moveJoystick.position.y ) );
		
		animator = GetComponent("Animator");
		var f_hor : float = 10*moveJoystick.position.x;
		var f_ver : float = 10*moveJoystick.position.y;
		var movimiento : float = f_hor*f_hor+f_ver*f_ver;
		animator.SetFloat("speed", movimiento);
		nw.RPC("activarCaminar",RPCMode.All, movimiento);
			
		// We only want horizontal movement
		movement.y = 0;
		movement.Normalize();

		var cameraTarget = Vector3.zero;

		// Apply movement from move joystick
		var absJoyPos = Vector2( Mathf.Abs( moveJoystick.position.x ), Mathf.Abs( moveJoystick.position.y ) );	
		if ( absJoyPos.y > absJoyPos.x )
		{
			if ( moveJoystick.position.y > 0 )
				movement *= forwardSpeed * absJoyPos.y;
			else
			{
				movement *= backwardSpeed * absJoyPos.y;
				cameraTarget.z = moveJoystick.position.y * 0.75;
			}
			
		}
		else
		{
			movement *= sidestepSpeed * absJoyPos.x;
			
			// Let's move the camera a bit, so the character isn't stuck under our thumb
			//cameraTarget.x = -moveJoystick.position.x * 0.5;
		}
		
		// Check for jump
		if ( character.isGrounded )
		{
			if ( rotateJoystick.tapCount == 2 )
			{
				// Apply the current movement to launch velocity		
				velocity = character.velocity;
				velocity.y = jumpSpeed;			
			}
		}
		else
		{			
			// Apply gravity to our velocity to diminish it over time
			velocity.y += Physics.gravity.y * Time.deltaTime;
			
			// Move the camera back from the character when we jump
			//cameraTarget.z = -jumpSpeed * 0.25;
			
			// Adjust additional movement while in-air
			movement.x *= inAirMultiplier;
			movement.z *= inAirMultiplier;
		}
			
		movement += velocity;	
		movement += Physics.gravity;
		movement *= Time.deltaTime;
		
		// Actually move the character	
		character.Move( movement );
		
		if ( character.isGrounded )
			// Remove any persistent velocity after landing	
			velocity = Vector3.zero;
		
		// Seek camera towards target position
		var pos = cameraPivot.localPosition;
		pos.x = Mathf.SmoothDamp( pos.x, cameraTarget.x, cameraVelocity.x, 0.3 );
		pos.z = Mathf.SmoothDamp( pos.z, cameraTarget.z, cameraVelocity.z, 0.5 );
		//cameraPivot.localPosition = pos;

		// Apply rotation from rotation joystick
		if ( character.isGrounded )
		{
			var camRotation = rotateJoystick.position;
			camRotation.x *= rotationSpeed.x;
			camRotation.y *= rotationSpeed.y;
			camRotation *= Time.deltaTime;
			
			// Rotate the character around world-y using x-axis of joystick
			thisTransform.Rotate( 0, camRotation.x, 0, Space.World );
			
			// Rotate only the camera with y-axis input
			//cameraPivot.Rotate( camRotation.y, 0, 0 );
		}
	}	
}
@RPC
function activarCaminar(valor : float )
{
	animator.SetFloat("speed", valor);
}