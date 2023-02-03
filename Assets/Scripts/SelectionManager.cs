using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
	[SerializeField]
	private Camera mainCamera;

	public LayerMask selectionMask;
	// Update is called once per frame

	public HexGrid hexGrid;
	private void Awake()
	{
		if (mainCamera == null)
			mainCamera = Camera.main;
	}
	void Update()
	{

	}

	public void HandleClick(Vector3 mousePos)
	{
		GameObject result;
		if (FindTarget(mousePos, out result))
		{
			Hex selectedInd = result.GetComponent<Hex>();
			List<Vector3Int> neighbours = hexGrid.GetNeighboursFor(selectedInd.HexCoords);
			foreach (Vector3Int neighbourPos in neighbours)
			{
				Debug.Log(neighbourPos);
			}
		}

	}

	private bool FindTarget(Vector3 mousePos, out GameObject result)
	{
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay(mousePos);
		if (Physics.Raycast(ray, out hit, selectionMask))
		{
			result = hit.collider.gameObject;
			return true;
		}
		result = null;
		return false;
	}
}
