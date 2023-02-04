using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private string P1, P2;
	public GameObject Ground;
	public Dictionary<Vector3Int, Tile> hexTileDict = new Dictionary<Vector3Int, Tile>();
	private GameState gameState;

	void Start()
	{
		hexTileDict = Ground.GetComponent<GenerateMap>().generateMap();
		P1 = "Player 1";
		P2 = "Player 2";
		gameState = GameState.P1TURN;
	}

	void Update()
	{
		// Debug.Log(hexTileDict[new Vector3Int(1, 0, -1)]);
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
			neighbours[i] = hexDict[cubePoint + directions[i]];
		}
		return neighbours;
	}

}

public enum TileState
{
	EMPTY, P1OCCUPIED, P2OCCUPIED, P1DECAYED, P2DECAYED, P1ROOT, P2ROOT
}

public enum GameState
{
	P1TURN, P2TURN, P1WIN, P2WIN, DRAW
}
