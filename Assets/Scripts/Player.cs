using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player leader;
	public static PlayerInput input;
	
	public delegate void state();
	public state currentState;
	public state nextState;
	
	float stepDelta;

	public Vector2 position;
	public Vector2 velocity;
	public float groundSpeed;
	public float groundAngle;
	
	public bool isLeader;
	public bool onGround;
	public bool rolling;
	
	bool left_lastFrame;
	bool right_lastFrame;
	bool up_lastFrame;
	bool down_lastFrame;
	bool jump_lastFrame;

	void StateMachine_Run(state stateMethod)
	{
		currentState = stateMethod;
		stateMethod();
	}
	
	void OnEnable()
	{
		nextState = Player_State_Grounded;
		
		// leader is the main player being controlled
		// only one player can be leader
		// other players that are not leader are treated as secondary characters
		if(isLeader)
		{
			if(leader == null)
			{
				leader = this;
			}
		}
		
		if(isLeader)
		{
			input = GetComponent<PlayerInput>();	
		}
	}

	void Start()
	{
		position = transform.position;
	}

	public void Player_Update(float deltaTime)
	{
		stepDelta = deltaTime;
		StateMachine_Run(nextState);
		
		Apply_Player_Movement();
		
		left_lastFrame  = input.left;
		right_lastFrame = input.right;
		up_lastFrame    = input.up;
		down_lastFrame  = input.down;
		jump_lastFrame  = input.jump;
	}

	public void Player_Late_Update() 
	{
		
	}

	void Player_State_Grounded() 
	{
		Handle_Ground_Movement();
		
		// if no longer grounded, switch to the airborne state
		if (!onGround) nextState = Player_State_Airborne;
	}
	
	void Player_State_Airborne() 
	{
		UpdatePosition();
		
		// if landed back on the ground, switch back to the grounded state
		if (onGround) nextState = Player_State_Grounded; 
	}
	
	void Handle_Ground_Movement()
	{
		// move the player down slopes based on how steep it is
		if(!rolling)
		{
			float slopeFactor = 0.125f;
			groundSpeed -= slopeFactor * Mathf.Sin(groundAngle * Mathf.Deg2Rad) * stepDelta;
		}
		
		Handle_Ground_Input();
		
		// set velocity to groundspeed in the direction of the ground angle
		velocity.x = groundSpeed * Mathf.Cos(groundAngle * Mathf.Deg2Rad);
		velocity.y = groundSpeed * Mathf.Sin(groundAngle * Mathf.Deg2Rad);
		
		UpdatePosition();
	}
	
	void Handle_Ground_Input()
	{
		if(input != null)
		{
			if(input.jump && !jump_lastFrame)
			{
				Debug.Log("jump");
			}
			
			float topSpeed = 6;
				
			if(input.left && !input.right)
			{
				if(groundSpeed < 0)
				{
					if(groundSpeed > -topSpeed)
					{
						groundSpeed -= 0.046875f * stepDelta;
					}
				}
				else
				{
					groundSpeed -= 0.5f * stepDelta;
					if(groundSpeed < 0) groundSpeed = -0.5f;
				}
			}
			
			if(input.right && !input.left)
			{
				if(groundSpeed > 0)
				{
					if(groundSpeed < topSpeed)
					{
						groundSpeed += 0.046875f * stepDelta;
					}
				}
				else
				{
					groundSpeed += 0.5f * stepDelta;
					if(groundSpeed > 0) groundSpeed = 0.5f;
				}
			}
			
			if(!input.right && !input.left)
			{
				// friction
				if(Mathf.Abs(groundSpeed) > 0)
				{
					float groundSpeedSign = Mathf.Sign(groundSpeed);
					groundSpeed -= 0.046875f * groundSpeedSign * stepDelta;
					
					if(Mathf.Sign(groundSpeed) != groundSpeedSign)
					{
						groundSpeed = 0;
					}
				}
			}
		}
	}

	// move the player by velocity
	void UpdatePosition()
	{
		position += velocity * stepDelta / 16f;
	}
	
	void Apply_Player_Movement()
	{
		transform.position = position;
	}
}
