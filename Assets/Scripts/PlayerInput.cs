using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[HideInInspector]public bool left;
	[HideInInspector]public bool right;
	[HideInInspector]public bool up;
	[HideInInspector]public bool down;
	[HideInInspector]public bool jump;
	
	void Update()
	{
		left = Input.GetKey(KeyCode.LeftArrow);
		right = Input.GetKey(KeyCode.RightArrow);
		up = Input.GetKey(KeyCode.UpArrow);
		down = Input.GetKey(KeyCode.DownArrow);
		jump = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S));
 	}
}
