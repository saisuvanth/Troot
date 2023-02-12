using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Coherence;
using Coherence.Toolkit;

public class PlayerInput : MonoBehaviour
{
	public new Camera camera;

	private CoherenceSync coherenceSync;
	private GameManager gameManager;

	private Vector3 _prevMousePos;

	void Start()
	{
		coherenceSync = GetComponent<CoherenceSync>();
		gameManager = GetComponent<GameManager>();
	}

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
			Debug.Log("click detected");
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				Debug.DrawLine(ray.origin, hit.point, Color.red, 10f);
				float posX = hit.point.x;
				float posZ = hit.point.z;
				Vector3Int cubePoint = GenerateMap.OddToCube(GenerateMap.WorldToOdd(hit.point));
				Vector3 cubePointV3 = new Vector3(cubePoint.x, cubePoint.y, cubePoint.z);
				coherenceSync.SendCommand<PlayerInput>(nameof(PlayerInput.onClick), MessageTarget.AuthorityOnly, cubePointV3, ray.origin, hit.point, GameManager.currentPlayer);
			}
		}
	}

	public void onClick(Vector3 cubePointV3, Vector3 start, Vector3 end, int currentPlayer)
	{
		Debug.DrawLine(start, end, Color.red, 10f);
		Player p = (Player)currentPlayer;
		Vector3Int cubePoint = new Vector3Int((int)cubePointV3.x, (int)cubePointV3.y, (int)cubePointV3.z);
		if (p == Player.P1 && gameManager.gameState == (int)GameState.P1TURN)
		{
			Debug.Log("clicked on P1");
			manageClick(cubePoint, GameState.P1TURN);
		}
		else if (p == Player.P2 && gameManager.gameState == (int)GameState.P2TURN)
		{
			Debug.Log("clicked on P2");
			manageClick(cubePoint, GameState.P2TURN);
		}

	}
	public void DecayRoots(Tile[] nTiles, Vector3Int cubePoint, GameState currentTurn)
	{
		foreach (var tile in nTiles)
		{
			if (tile != null && (tile.state == (currentTurn == GameState.P1TURN ? TileState.P2OCCUPIED : currentTurn == GameState.P2TURN ? TileState.P1OCCUPIED : TileState.EMPTY)))
			{
				tile.state = currentTurn == GameState.P1TURN ? TileState.P2DECAYED : currentTurn == GameState.P2TURN ? TileState.P1DECAYED : tile.state;
				tile.transform.GetComponent<TileData>().tileUpdate(gameManager.hexTileDict);
			}
		}
	}

	public void manageClick(Vector3Int cubePoint, GameState currentTurn)
	{
		Debug.Log("clicked on other client " + cubePoint);
		// GameState currentTurn = (GameState)gameManager.gameState;
		if (gameManager.hexTileDict.ContainsKey(cubePoint))
		{
			if (gameManager.hexTileDict[cubePoint].state == TileState.EMPTY)
			{
				Tile[] nTiles = gameManager.hexTileDict[cubePoint].GetNeighbours(gameManager.hexTileDict, cubePoint);
				foreach (var tile in nTiles)
				{
					if (tile != null && (tile.state == (currentTurn == GameState.P1TURN ? TileState.P1OCCUPIED : TileState.P2OCCUPIED) || tile.state == (currentTurn == GameState.P1TURN ? TileState.P1ROOT : TileState.P2ROOT)))
					{
						gameManager.hexTileDict[cubePoint].state = currentTurn == GameState.P1TURN ? TileState.P1OCCUPIED : TileState.P2OCCUPIED;
						gameManager.gameState = (int)(currentTurn == GameState.P1TURN ? GameState.P2TURN : GameState.P1TURN);
						tile.transform.GetComponent<TileData>().tileUpdate(gameManager.hexTileDict);
					}
				}
				GameObject obj = GameObject.Find(string.Format("Tile -> {0} {1} {2}", cubePoint.x, cubePoint.y, cubePoint.z));
				obj.GetComponent<TileData>().tileUpdate(gameManager.hexTileDict);
				DecayRoots(nTiles, cubePoint, currentTurn);
				gameManager.isDecayed();
				gameManager.updateScore();
			}
		}
	}
}
