using System.Collections.Generic;
using UnityEngine;
using Coherence.Toolkit;
using System;

public class TileData : MonoBehaviour
{
	public List<GameObject> meshes;

	// public Transform[] arr;
	public Material redMat;
	[Sync]
	public int state;
	[Sync]
	public int orientation;
	[Sync]
	public int tileState;
	public bool selected = false;
	// public GameObject gameManager;
	private GameManager gameManager;

	private GameObject empty;
	private Vector3Int cubePoint;

	// Start is called before the first frame update
	public void Start()
	{
		empty = GameObject.Find("empty");
		state = TileDataState.EMPTY;
		cubePoint = GenerateMap.OddToCube(GenerateMap.WorldToOdd(transform.position));
		gameManager = FindObjectOfType<GameManager>();
		// gamManScript = gameManager.GetComponent<GameManager>();
	}

	bool isFavourable(TileState k, bool isP1)
	{
		if (isP1)
		{
			if (k == TileState.P1OCCUPIED || k == TileState.P1ROOT) return true;
		}
		else
		{
			if (k == TileState.P2OCCUPIED || k == TileState.P2ROOT) return true;
		}
		return false;
	}


	void SetStateAndOrientation(Dictionary<Vector3Int, Tile> hexTileDict)
	{
		int neighbours = 0;
		Vector3Int vec_tl = (cubePoint + new Vector3Int(0, -1, 1));
		Vector3Int vec_tr = (cubePoint + new Vector3Int(1, -1, 0));
		Vector3Int vec_l = (cubePoint + new Vector3Int(-1, 0, 1));
		Vector3Int vec_r = (cubePoint + new Vector3Int(1, 0, -1));
		Vector3Int vec_bl = (cubePoint + new Vector3Int(-1, 1, 0));
		Vector3Int vec_br = (cubePoint + new Vector3Int(0, 1, -1));
		bool is_tl = false, is_tr = false, is_l = false, is_r = false, is_bl = false, is_br = false;
		for (int q = cubePoint.x - 1; q <= cubePoint.x + 1; q++)
		{
			for (int r = cubePoint.y - 1; r <= cubePoint.y + 1; r++)
			{
				for (int s = cubePoint.z - 1; s <= cubePoint.z + 1; s++)
				{
					Vector3Int temp = new Vector3Int(q, r, s);
					if (q + r + s == 0 && hexTileDict.ContainsKey(temp) && temp != cubePoint)
					{
						if (isFavourable(hexTileDict[temp].state, true) && isFavourable(hexTileDict[cubePoint].state, true))
						{
							if (temp == vec_tl) is_tl = true;
							else if (temp == vec_tr) is_tr = true;
							else if (temp == vec_l) is_l = true;
							else if (temp == vec_r) is_r = true;
							else if (temp == vec_bl) is_bl = true;
							else if (temp == vec_br) is_br = true;
							neighbours++;
						}
						if (isFavourable(hexTileDict[temp].state, false) && isFavourable(hexTileDict[cubePoint].state, false))
						{
							if (temp == vec_tl) is_tl = true;
							else if (temp == vec_tr) is_tr = true;
							else if (temp == vec_l) is_l = true;
							else if (temp == vec_r) is_r = true;
							else if (temp == vec_bl) is_bl = true;
							else if (temp == vec_br) is_br = true;
							neighbours++;
						}
					}
				}
			}
		}

		switch (neighbours)
		{
			case 6:
				state = TileDataState.D6_1;
				break;
			case 5:
				state = TileDataState.D5_1;
				if (!is_tr) orientation = 0;
				else if (!is_r) orientation = 60;
				else if (!is_br) orientation = 120;
				else if (!is_bl) orientation = 180;
				else if (!is_l) orientation = 240;
				else if (!is_tl) orientation = 300;
				else state = TileDataState.EMPTY;
				break;
			case 4:
				if ((!is_tl && !is_tr) || (!is_tr && !is_r) || (!is_r && !is_br) || (!is_br && !is_bl) || (!is_bl && !is_l) || (!is_l && !is_tl))
				{
					state = TileDataState.D4_1;
					if (!is_tl && !is_tr) orientation = 0;
					else if (!is_tr && !is_r) orientation = 60;
					else if (!is_r && !is_br) orientation = 120;
					else if (!is_br && !is_bl) orientation = 180;
					else if (!is_bl && !is_l) orientation = 240;
					else if (!is_l && !is_tl) orientation = 300;
				}
				else if ((!is_tr && !is_br) || (!is_r && !is_bl) || (!is_br && !is_l) || (!is_bl && !is_tl) || (!is_l && !is_tr) || (!is_tl && !is_r))
				{
					state = TileDataState.D4_2;
					if (!is_tr && !is_br) orientation = 0;
					else if (!is_r && !is_bl) orientation = 60;
					else if (!is_br && !is_l) orientation = 120;
					else if (!is_bl && !is_tl) orientation = 180;
					else if (!is_l && !is_tr) orientation = 240;
					else if (!is_tl && !is_r) orientation = 300;
				}
				else if ((!is_tr && !is_bl) || (!is_r && !is_l) || (!is_br && !is_tl))
				{
					state = TileDataState.D4_3;
					if (!is_tr && !is_bl) orientation = 0;
					else if (!is_r && !is_l) orientation = 60;
					else if (!is_br && !is_tl) orientation = 120;
				}
				else state = TileDataState.EMPTY;
				break;
			case 3:
				if ((is_r && is_br && is_bl) || (is_br && is_bl && is_l) || (is_bl && is_l && is_tl) || (is_l && is_tl && is_tr) || (is_tl && is_tr && is_r) || (is_tr && is_r && is_br))
				{
					state = TileDataState.D3_1;
					if (is_r && is_br && is_bl) orientation = 0;
					else if (is_br && is_bl && is_l) orientation = 60;
					else if (is_bl && is_l && is_tl) orientation = 120;
					else if (is_l && is_tl && is_tr) orientation = 180;
					else if (is_tl && is_tr && is_r) orientation = 240;
					else if (is_tr && is_r && is_br) orientation = 300;
				}

				else if ((is_r && is_bl && is_l) || (is_br && is_l && is_tl) || (is_bl && is_tl && is_tr) || (is_l && is_tr && is_r) || (is_tl && is_r && is_br) || (is_tr && is_br && is_bl))
				{
					state = TileDataState.D3_2;
					if (is_r && is_bl && is_l) orientation = 0;
					else if (is_br && is_l && is_tl) orientation = 60;
					else if (is_bl && is_tl && is_tr) orientation = 120;
					else if (is_l && is_tr && is_r) orientation = 180;
					else if (is_tl && is_r && is_br) orientation = 240;
					else if (is_tr && is_br && is_bl) orientation = 300;
				}
				else if ((is_r && is_l && is_tl) || (is_br && is_tl && is_tr) || (is_bl && is_tr && is_r) || (is_l && is_r && is_br) || (is_tl && is_br && is_bl) || (is_tr && is_bl && is_l))
				{
					state = TileDataState.D3_3;
					if (is_r && is_l && is_tl) orientation = 0;
					else if (is_br && is_tl && is_tr) orientation = 60;
					else if (is_bl && is_tr && is_r) orientation = 120;
					else if (is_l && is_r && is_br) orientation = 180;
					else if (is_tl && is_br && is_bl) orientation = 240;
					else if (is_tr && is_bl && is_l) orientation = 300;
				}
				else if ((is_r && is_bl && is_tl) || (is_br && is_l && is_tr))
				{
					state = TileDataState.D3_4;
					if (is_r && is_bl && is_tl) orientation = 0;
					else if (is_br && is_l && is_tr) orientation = 60;
				}
				else state = TileDataState.EMPTY;
				break;
			case 2:
				if ((is_tl && is_tr) || (is_tr && is_r) || (is_r && is_br) || (is_br && is_bl) || (is_bl && is_l) || (is_l && is_tl))
				{
					state = TileDataState.D2_1;
					if (is_r && is_br) orientation = 0;
					else if (is_br && is_bl) orientation = 60;
					else if (is_bl && is_l) orientation = 120;
					else if (is_l && is_tl) orientation = 180;
					else if (is_tl && is_tr) orientation = 240;
					else if (is_tr && is_r) orientation = 300;
				}
				else if ((is_r && is_bl) || (is_bl && is_tl) || (is_tl && is_r) || (is_l && is_br) || (is_br && is_tr) || (is_tr && is_l))
				{
					state = TileDataState.D2_2;
					if (is_r && is_bl) orientation = 0;
					else if (is_br && is_l) orientation = 60;
					else if (is_bl && is_tl) orientation = 120;
					else if (is_l && is_tr) orientation = 180;
					else if (is_tl && is_r) orientation = 240;
					else if (is_tr && is_br) orientation = 300;
				}
				else if ((is_l && is_r) || (is_tl && is_br) || (is_tr && is_bl))
				{
					state = TileDataState.D2_3;
					if (is_r && is_l) orientation = 0;
					else if (is_br && is_tl) orientation = 60;
					else if (is_bl && is_tr) orientation = 120;
				}
				else state = TileDataState.EMPTY;
				break;
			case 1:
				state = TileDataState.D1_1;
				if (is_r) orientation = 0;
				else if (is_br) orientation = 60;
				else if (is_bl) orientation = 120;
				else if (is_l) orientation = 180;
				else if (is_tl) orientation = 240;
				else if (is_tr) orientation = 300;
				else state = TileDataState.EMPTY;
				break;
			default:
				state = TileDataState.EMPTY;
				break;
		}

	}

	// Update is called once per frame
	public void tileUpdate(Dictionary<Vector3Int, Tile> hexTileDict)
	{
		tileState = (int)hexTileDict[cubePoint].state;
		if (hexTileDict[cubePoint].state != TileState.EMPTY) SetStateAndOrientation(hexTileDict);

	}

	public void Update()
	{
		UpdateCurrentMesh();
	}

	private List<string> meshMap = new List<string>()
  {
	"empty",
	"1_1",
	"2_1",
	"2_2",
	"2_3",
	"3_1",
	"3_2",
	"3_3",
	"3_4",
	"4_1",
	"4_2",
	"4_3",
	"5_1",
	"6_1",
	"root"
  };

	public void UpdateCurrentMesh()
	{

		// CombineInstance[] combine = new CombineInstance[2];
		// combine[0].mesh = empty.GetComponent<MeshFilter>().mesh;
		// combine[1].mesh = GameObject.Find(meshMap[state]).GetComponent<MeshFilter>().mesh;
		// GetComponent<MeshFilter>().mesh = new Mesh();
		// GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		try
		{

			GameObject temp = GameObject.Find(meshMap[state]);
			GetComponent<MeshFilter>().mesh = temp.GetComponent<MeshFilter>().mesh;
			transform.rotation = Quaternion.Euler(0, orientation, 0);
			switch ((TileState)tileState)
			{
				case TileState.P1OCCUPIED:
				case TileState.P1ROOT:
					GetComponent<MeshRenderer>().materials[1].color = Color.blue;
					break;
				case TileState.P2OCCUPIED:
				case TileState.P2ROOT:
					GetComponent<MeshRenderer>().materials[1].color = Color.red;
					break;
				case TileState.P1DECAYED:
				case TileState.P2DECAYED:
					GetComponent<MeshRenderer>().materials[1].color = Color.gray;
					break;
				default:
					break;
			}
			// GetComponent<MeshRenderer>().materials = temp.GetComponent<MeshRenderer>().materials;
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}
}



public class TileDataState
{
	public static int EMPTY = 0;
	public static int D1_1 = 1;
	public static int D2_1 = 2;
	public static int D2_2 = 3
	;
	public static int D2_3 = 4
	;
	public static int D3_1 = 5
	;
	public static int D3_2 = 6
	;
	public static int D3_3 = 7
	;
	public static int D3_4 = 8
	;
	public static int D4_1 = 9
	;
	public static int D4_2 = 10
	;
	public static int D4_3 = 11
	;
	public static int D5_1 = 12
	;
	public static int D6_1 = 13;
	public static int ROOT = 14;

}