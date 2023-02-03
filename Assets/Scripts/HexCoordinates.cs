using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset=1f,yOffset=1,zOffset=0.86f;

    internal Vector3Int GetHexCoords() => offsetCoordinates;

    [Header("Offset Coordinates")]
    [SerializeField] 
    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertToOffsetCoordinates(transform.position);
    }

    private Vector3Int ConvertToOffsetCoordinates(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);
        return new Vector3Int(x, y, z);
    }
}
