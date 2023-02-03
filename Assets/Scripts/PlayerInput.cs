using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
	public UnityEvent<Vector3> PointerClick;

	// Update is called once per frame 
	void Update()
	{
		DetectClick();
	}

	private void DetectClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousPos = Input.mousePosition;
			PointerClick.Invoke(mousPos);
		}
	}
}
