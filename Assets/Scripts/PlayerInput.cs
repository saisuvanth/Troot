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
				GameManager gameManager = gameObject.GetComponent<GameManager>();
				Vector3Int cubePoint = GenerateMap.OddToCube(GenerateMap.WorldToOdd(hit.point));
				GameState state = gameManager.gameState;
				if (gameManager.hexTileDict.ContainsKey(cubePoint))
				{
					if (gameManager.hexTileDict[cubePoint].state == TileState.EMPTY)
					{
						Tile[] nTiles = gameManager.hexTileDict[cubePoint].GetNeighbours(gameManager.hexTileDict, cubePoint);
						foreach (var tile in nTiles)
						{
							if (tile != null && (tile.state == (state == GameState.P1TURN ? TileState.P1OCCUPIED : TileState.P2OCCUPIED) || tile.state == (state == GameState.P1TURN ? TileState.P1ROOT : TileState.P2ROOT)))
							{
								gameManager.hexTileDict[cubePoint].state = state == GameState.P1TURN ? TileState.P1OCCUPIED : TileState.P2OCCUPIED;
								gameManager.gameState = state == GameState.P1TURN ? GameState.P2TURN : GameState.P1TURN;
								break;
							}
						}
					}
				}
			}
		}
	}
}
