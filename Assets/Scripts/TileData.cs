using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;


public class TileData : MonoBehaviour
{
	public Transform[] arr;
	public Material redMat;
	public string state;
	public float orientation;
	public bool selected = false;
	public GameObject gameManager;
	public GameManager gameManScript;
	private Vector3Int cubePoint;

	public MeshRenderer emptyRenderer;
	// Start is called before the first frame update
	void Start()
	{
		arr = this.GetComponentsInChildren<Transform>(true);
		state = TileDataState.EMPTY;
		cubePoint = GenerateMap.OddToCube(GenerateMap.WorldToOdd(transform.position));
		gameManScript = FindObjectOfType<GameManager>();
		// gamManScript = gameManager.GetComponent<GameManager>();
	}


	void CalculateTileDataState()
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
					if (q + r + s == 0 && gameManScript.hexTileDict.ContainsKey(temp) && temp != cubePoint)
					{
						if (gameManScript.hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1OCCUPIED || gameManScript.hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2OCCUPIED)
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
	void Update()
	{
		switch (gameManScript.hexTileDict[cubePoint].state)
		{
			case TileState.P1OCCUPIED:
			case TileState.P1ROOT:
				emptyRenderer.materials[1].color = Color.blue;
				break;
			case TileState.P2OCCUPIED:
			case TileState.P2ROOT:
				emptyRenderer.materials[1].color = Color.red;
				break;
			default:
				break;
		}
		if (gameManScript.hexTileDict[cubePoint].state != TileState.EMPTY) CalculateTileDataState();
		foreach (var obj in arr)
		{
			if (obj == transform) continue;
			obj.transform.rotation = Quaternion.Euler(0, orientation, 0);
			if (gameManScript.hexTileDict[cubePoint].state == TileState.P1ROOT || gameManScript.hexTileDict[cubePoint].state == TileState.P2ROOT)
			{
				// state=TileDataState.D_1;
				if (obj.name == TileDataState.ROOT) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			if (state == TileDataState.D1_1)
			{
				if (obj.name == TileDataState.D1_1) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D2_1)
			{
				if (obj.name == TileDataState.D2_1) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D2_2)
			{
				if (obj.name == TileDataState.D2_2) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D2_3)
			{
				if (obj.name == TileDataState.D2_3) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D3_1)
			{
				if (obj.name == TileDataState.D3_1) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D3_2)
			{
				if (obj.name == TileDataState.D3_2) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D3_3)
			{
				if (obj.name == TileDataState.D3_3) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D4_1)
			{
				if (obj.name == TileDataState.D4_1) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D4_2)
			{
				if (obj.name == TileDataState.D4_2) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D4_3)
			{
				if (obj.name == TileDataState.D4_3) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D5_1)
			{
				if (obj.name == TileDataState.D5_1) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			else if (state == TileDataState.D6_1)
			{
				if (obj.name == TileDataState.D6_1) obj.gameObject.SetActive(true);
				else obj.gameObject.SetActive(false);
			}
			if (obj.name == TileDataState.EMPTY) obj.gameObject.SetActive(true);
		}
	}
}


public class TileDataState
{
	public static string EMPTY = "empty";
	public static string D1_1 = "1_1";
	public static string D2_1 = "2_1";
	public static string D2_2 = "2_2"
	;
	public static string D2_3 = "2_3"
	;
	public static string D3_1 = "3_1"
	;
	public static string D3_2 = "3_2"
	;
	public static string D3_3 = "3_3"
	;
	public static string D3_4 = "3_4"
	;
	public static string D4_1 = "4_1"
	;
	public static string D4_2 = "4_2"
	;
	public static string D4_3 = "4_3"
	;
	public static string D5_1 = "5_1"
	;
	public static string D6_1 = "6_1";
	public static string ROOT = "root";

}