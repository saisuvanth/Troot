using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Coherence.Runtime;
using Coherence.Toolkit;
using System;

public class GameManager : MonoBehaviour
{
	public CoherenceMonoBridge bridge;

	public GameObject Ground;

	public Dictionary<Vector3Int, Tile> hexTileDict;
	public int gameState;
	public static int currentPlayer = (int)Player.P2;
	public int P1score;
	public int P2score;
	public int maxScore;

	void Awake()
	{
		if (!MonoBridgeStore.TryGetBridge(gameObject.scene, out bridge))
		{
			return;
		}
		RoomData rm = RoomScript.joinedRoomData;
		Debug.Log(bridge);
		JoinRoom(rm);
	}

	void Start()
	{
		hexTileDict = Ground.GetComponent<GenerateMap>().populateDictionary();
		gameState = (int)GameState.P1TURN;
	}



	public void JoinRoom(RoomData roomData)
	{
		Debug.Log(bridge);
		try
		{
			bridge.JoinRoom(roomData);
			bridge.onConnected.AddListener(onConnected);
			Debug.Log("Room Data: " + roomData);
			// Scene transition with room id
		}
		catch (System.Exception e)
		{
			Debug.Log("Error: " + e);
		}
	}

	private void onConnected(CoherenceMonoBridge arg0)
	{
		Debug.Log(arg0.ClientConnections.ClientConnectionCount);
	}

	public async void LeaveRoom()
	{
		// @saisuvanth @FreSauce check that the current user is the creator. If not, then don't allow them to leave the room.

		if (RoomScript.selectedRegion == "")
		{
			return;
		}

		try
		{
			Debug.Log("RoomScript.selectedRegion: " + RoomScript.selectedRegion);
			Debug.Log("RoomScript.joinedRoomData.Id: " + RoomScript.joinedRoomData.UniqueId);
			Debug.Log("RoomScript.joinedRoomData.Secret: " + RoomScript.joinedRoomData.Secret);
			await PlayResolver.RemoveRoom(RoomScript.selectedRegion, RoomScript.joinedRoomData.UniqueId, RoomScript.joinedRoomData.Secret);
		}
		catch (System.Exception e)
		{
			Debug.Log("Error: " + e);
		}

		UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
	}


	public void updateScore()
	{
		int mapLength = GenerateMap.mapLength;

		P1score = 0;
		P2score = 0;
		maxScore = 0;

		for (int q = -mapLength; q <= mapLength; q++)
		{
			for (int r = -mapLength; r <= mapLength; r++)
			{
				for (int s = -mapLength; s <= mapLength; s++)
				{
					if (q + r + s == 0)
					{
						if ((hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1OCCUPIED) || (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1ROOT))
						{
							P1score++;
						}

						if ((hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2OCCUPIED) || (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2ROOT))
						{
							P2score++;
						}

						maxScore++;
					}

				}
			}
		}
	}

	public void isDecayed()
	{
		int mapLength = GenerateMap.mapLength;

		for (int q = -mapLength; q <= mapLength; q++)
		{
			for (int r = -mapLength; r <= mapLength; r++)
			{
				for (int s = -mapLength; s <= mapLength; s++)
				{
					if (q + r + s == 0)
					{
						if (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1OCCUPIED)
						{
							if (!findRoot(hexTileDict[new Vector3Int(q, r, s)], Player.P1))
							{
								hexTileDict[new Vector3Int(q, r, s)].state = TileState.P1DECAYED;
							}
						}

						if (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2OCCUPIED)
						{
							if (!findRoot(hexTileDict[new Vector3Int(q, r, s)], Player.P2))
							{
								hexTileDict[new Vector3Int(q, r, s)].state = TileState.P2DECAYED;
							}
						}
					}
				}
			}
		}
	}

	public bool findRoot(Tile src, Player currentPlayer)
	{
		Debug.Log("findRoot");
		// Add a queue of Vector3Ints
		Queue<Tile> queue = new Queue<Tile>();

		// Maintain a list of visited nodes
		Dictionary<Tile, bool> visited = new Dictionary<Tile, bool>();

		// Add the source to the queue
		queue.Enqueue(src);

		// Mark the source as visited
		visited[src] = true;

		// While the queue is not empty
		while (queue.Count > 0)
		{
			// Dequeue the first element
			Tile current = queue.Dequeue();
			Debug.Log("BFS: " + current);

			// If the current element is the root, return true
			if (currentPlayer == Player.P1 && current.state == TileState.P1ROOT)
			{
				Debug.Log("found root");
				return true;
			}

			if (currentPlayer == Player.P2 && current.state == TileState.P2ROOT)
			{
				Debug.Log("found root");
				return true;
			}

			// Get the neighbours of the current element
			Tile[] neighbours = current.GetNeighbours(hexTileDict, current.WorldToTile());

			// For each neighbour
			for (int i = 0; i < neighbours.Length; i++)
			{

				// If the neighbour is not null
				if (neighbours[i] != null && !visited.ContainsKey(neighbours[i]))
				{
					if (currentPlayer == Player.P1 && (neighbours[i].state == TileState.P1OCCUPIED || neighbours[i].state == TileState.P1ROOT))
					{
						// Add the neighbour to the queue
						queue.Enqueue(current);
						visited[current] = true;
					}

					if (currentPlayer == Player.P2 && (neighbours[i].state == TileState.P2OCCUPIED || neighbours[i].state == TileState.P2ROOT))
					{
						// Add the neighbour to the queue
						queue.Enqueue(current);
						visited[current] = true;
					}
				}
			}
		}


		return false;
	}


}

public class Tile
{
	public Transform transform { get; set; }
	public TileState state { get; set; }

	public Tile(Transform transform, TileState state)
	{
		this.transform = transform;
		this.state = state;
	}

	public Tile[] GetNeighbours(Dictionary<Vector3Int, Tile> hexDict, Vector3Int cubePoint)
	{
		Tile[] neighbours = new Tile[6];
		Vector3Int[] directions = new Vector3Int[6]{
	  new Vector3Int(1, 0, -1),
	  new Vector3Int(1, -1, 0),
	  new Vector3Int(0, -1, 1),
	  new Vector3Int(-1, 0, 1),
	  new Vector3Int(-1, 1, 0),
	  new Vector3Int(0, 1, -1)
	};
		for (int i = 0; i < 6; i++)
		{
			if (hexDict.ContainsKey(cubePoint + directions[i]))
			{
				neighbours[i] = hexDict[cubePoint + directions[i]];
			}
			else
			{
				neighbours[i] = null;
			}
		}
		return neighbours;
	}

	public Vector3Int WorldToTile()
	{
		Vector3 worldCoordinate = transform.position;
		Vector3Int odd = GenerateMap.WorldToOdd(worldCoordinate);
		return GenerateMap.OddToCube(odd);
	}

}

public enum TileState
{
	EMPTY, P1OCCUPIED, P2OCCUPIED, P1DECAYED, P2DECAYED, P1ROOT, P2ROOT

}

public enum Player
{
	P1, P2
}

public enum GameState
{
	P1TURN = 1, P2TURN = 2, P1WIN = 3, P2WIN = 4, DRAW = 5
}
