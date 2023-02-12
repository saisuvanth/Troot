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
	public Vector3Int p1Root;
	public Vector3Int p2Root;

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

		// Set the root tiles
		foreach (KeyValuePair<Vector3Int, Tile> entry in hexTileDict)
		{
			if (entry.Value.state == TileState.P1ROOT)
			{
				p1Root = entry.Key;
			}
			else if (entry.Value.state == TileState.P2ROOT)
			{
				p2Root = entry.Key;
			}
		}

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

		Dictionary<Vector3Int, bool> isOccupiedByP1=new Dictionary<Vector3Int, bool>();
		Dictionary<Vector3Int, bool> isOccupiedByP2=new Dictionary<Vector3Int, bool>();

		foreach (KeyValuePair<Vector3Int, Tile> entry in hexTileDict)
		{
			if (entry.Value.state == TileState.P1OCCUPIED)
			{
				isOccupiedByP1[entry.Key] = true;
			}
			if (entry.Value.state == TileState.P2OCCUPIED)
			{
				isOccupiedByP2[entry.Key] = true;
			}
		}

		checkForDecay(p1Root,ref isOccupiedByP1,Player.P1);
		checkForDecay(p2Root,ref isOccupiedByP2,Player.P2);

		foreach(KeyValuePair<Vector3Int,bool> entry in isOccupiedByP1)
		{
			hexTileDict[entry.Key].state=TileState.P1DECAYED;
			hexTileDict[entry.Key].transform.GetComponent<TileData>().tileUpdate(hexTileDict);
		}

		foreach(KeyValuePair<Vector3Int,bool> entry in isOccupiedByP2)
		{

			hexTileDict[entry.Key].state=TileState.P2DECAYED;
			hexTileDict[entry.Key].transform.GetComponent<TileData>().tileUpdate(hexTileDict);
		}

	}

	public void checkForDecay(Vector3Int src,ref Dictionary<Vector3Int, bool> isOccupiedDict,Player currentPlayer)
	{
		Queue<Vector3Int> q = new Queue<Vector3Int>();

		Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();

		q.Enqueue(src);
		visited[src] = true;

		while (q.Count > 0)
		{
			Vector3Int u = q.Dequeue();
			Debug.Log("BFS : "+q);

			Tile[] neighbours = hexTileDict[u].GetNeighbours(hexTileDict, u);

			foreach (Tile v in neighbours)
			{
				if(v==null)
				{
					continue;
				}

				Vector3Int vCube = v.WorldToTile();

				if (!visited.ContainsKey(vCube)&&isOccupiedDict.ContainsKey(vCube))
				{
					isOccupiedDict.Remove(vCube);
					q.Enqueue(vCube);
					visited[vCube] = true;
				}
			}
		}

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
