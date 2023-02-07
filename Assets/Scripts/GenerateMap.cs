using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	// Start is called before the first frame update
	public GameObject prefabTile;
	public GameObject prefabTile1;
	public GameObject prefabTile2;
	public GameObject prefabTile3;
	public GameObject prefabTile4;
	public GameObject prefabTile5;
	public GameObject prefabTile6;
	public int mapLength;
	private float size = 1f / Mathf.Sqrt(3f);

	public Dictionary<Vector3Int, Tile> generateMap()
	{
		Dictionary<Vector3Int, Tile> hexTiles = new Dictionary<Vector3Int, Tile>();
		for (int q = -mapLength; q <= mapLength; q++)
		{
			for (int r = -mapLength; r <= mapLength; r++)
			{
				for (int s = -mapLength; s <= mapLength; s++)
				{
					if (q + r + s == 0)
					{
						Vector2Int axes = CubeToOdd(new Vector3Int(q, r, s));
						int y = axes.y;
						int x = axes.x;
						Vector3 newPosition = OddToWorld(new Vector2Int(x, y));
						if (q == mapLength && r == -mapLength)
						{
							GameObject tile = Instantiate(prefabTile, newPosition, Quaternion.identity, this.transform);
							tile.name = "Tile -> " + x + " " + y;
							hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.P2OCCUPIED));
						}
						else if (q == -mapLength && r == mapLength)
						{
							GameObject tile = Instantiate(prefabTile, newPosition, Quaternion.identity, this.transform);
							tile.name = "Tile -> " + x + " " + y;
							hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.P1OCCUPIED));
						}
						else
						{
							// Generate a random number between 0 and 2
							int random = UnityEngine.Random.Range(0, 25);
							if (random == 0)
							{
								GameObject tile = Instantiate(prefabTile1, newPosition, Quaternion.identity, this.transform);
								tile.name = "Tile -> " + x + " " + y;
								hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.FILLED));
							}
							else if (random == 1)
							{
								GameObject tile = Instantiate(prefabTile2, newPosition, Quaternion.identity, this.transform);
								tile.name = "Tile -> " + x + " " + y;
								hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.FILLED));
							}
							else if (random == 2)
							{
								GameObject tile = Instantiate(prefabTile3, newPosition, Quaternion.identity, this.transform);
								tile.name = "Tile -> " + x + " " + y;
								hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.FILLED));
							}
							else if (random == 3)
							{
								int random2 = UnityEngine.Random.Range(0, 2);

								if (random2 % 2 == 0)
								{
									GameObject tile = Instantiate(prefabTile4, newPosition, Quaternion.identity, this.transform);
									tile.name = "Tile -> " + x + " " + y;
									hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.FILLED));
								}
								else
								{
									GameObject tile = Instantiate(prefabTile, newPosition, Quaternion.identity, this.transform);
									tile.name = "Tile -> " + x + " " + y;
									hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.EMPTY));
								}

							}
							else if (random == 4)
							{
								GameObject tile = Instantiate(prefabTile5, newPosition, Quaternion.identity, this.transform);
								tile.name = "Tile -> " + x + " " + y;
								hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.FILLED));
							}
							else if (random == 5)
							{
								GameObject tile = Instantiate(prefabTile6, newPosition, Quaternion.identity, this.transform);
								tile.name = "Tile -> " + x + " " + y;
								hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.FILLED));
							}
							else
							{
								GameObject tile = Instantiate(prefabTile, newPosition, Quaternion.identity, this.transform);
								tile.name = "Tile -> " + x + " " + y;
								hexTiles.Add(new Vector3Int(q, r, s), new Tile(tile.transform, TileState.EMPTY));
							}
						}
					}
				}
			}
		}
		return hexTiles;
	}


	public static Vector3Int OddToCube(Vector3Int axes)
	{
		int col = axes.x;
		int row = axes.z;
		int q = col - (row - (row & 1)) / 2;
		int r = row;
		return new Vector3Int(q, r, -q - r);
	}

	public static Vector2Int CubeToOdd(Vector3Int qrs)
	{
		int q = qrs.x;
		int r = qrs.y;
		int col = q + (r - (r & 1)) / 2;
		int row = r;
		return new Vector2Int(col, row);
	}




	public static Vector3 OddToWorld(Vector2Int coordinate)
	{
		int column = coordinate.x;
		int row = coordinate.y;
		float width;
		float height;
		float xPosition;
		float yPosition;
		bool shouldOffset;
		float horizontalDistance;
		float verticalDistance;
		float offset;
		float size = 1f / Mathf.Sqrt(3f);


		shouldOffset = (Math.Abs(row) % 2) == 1;
		height = 2f * size;
		width = Mathf.Sqrt(3f) * size;



		verticalDistance = height * (3f / 4f);
		horizontalDistance = width;

		offset = (shouldOffset) ? width / 2 : 0;
		xPosition = (column * horizontalDistance) + offset;
		yPosition = (row * verticalDistance);
		return new Vector3(xPosition, 0, -yPosition);
	}

	public static Vector3Int WorldToOdd(Vector3 worldCoordinate)
	{
		float x = worldCoordinate.x;
		float z = -worldCoordinate.z;
		float size = 1f / Mathf.Sqrt(3f);
		float width = Mathf.Sqrt(3f) * size;
		float height = 2f * size;
		float horizontalDistance = width;
		float verticalDistance = height * (3f / 4f);
		int row = (int)Math.Round(z / verticalDistance, MidpointRounding.AwayFromZero);
		bool shouldOffset = (Math.Abs(row) % 2) == 1;
		float offset = (shouldOffset) ? width / 2 : 0;
		int column = (int)Math.Round((x - offset) / horizontalDistance, MidpointRounding.AwayFromZero);
		return new Vector3Int(column, 0, row);
	}


	// Update is called once per frame
	void Update()
	{

	}
}
