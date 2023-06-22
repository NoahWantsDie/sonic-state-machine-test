using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
	public static PhysicsManager Instance;
	public int substeps;
	public Player[] players;

	void OnEnable() 
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance != this)
			Destroy(this);
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		players = FindObjectsOfType<Player>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float substepDelta = Time.fixedDeltaTime * 60f / substeps;
		for (int i = 0; i < substeps; i ++)
		{
			foreach (Player player in players)
			{
				player.Player_Update(substepDelta);
				player.Player_Late_Update();
			}
		}
	}
}
