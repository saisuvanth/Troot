using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		if (GameManager.currentPlayer == (int)Player.P2)
		{
			transform.Rotate(0, 180, 0);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
