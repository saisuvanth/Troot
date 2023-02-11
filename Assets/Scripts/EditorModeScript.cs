using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorModeScript : MonoBehaviour
{
	public bool generateTiles;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (generateTiles)
		{
			generateTiles = false;
			FindObjectOfType<GenerateMap>().generateMap();
		}
	}


}
