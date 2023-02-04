using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
	public new Camera camera;


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
			Ray ray = camera.ScreenPointToRay(mousPos);
			ray.origin = camera.transform.position;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				float posX = hit.point.x;
				float posZ = hit.point.z;
				// Debug.Log(posX + ' ' + posZ);
				GameManager gameManager = gameObject.GetComponent<GameManager>();
				// GenerateMap.OddToCube(GenerateMap.WorldToOdd(hit.point));
				Vector3Int cubePoint = GenerateMap.OddToCube(GenerateMap.WorldToOdd(hit.point));
				if (gameManager.hexTileDict.ContainsKey(cubePoint))
				{
					// Debug.Log("gehe");
					// Debug.Log(cubePoint);
					gameManager.hexTileDict[cubePoint].state = TileState.P1OCCUPIED;
				}
			}
		}
	}
}
