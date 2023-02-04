using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefabTile;
    public int mapLength;
    public float tileWidth = 1f;
    public float tileHeight = 2f;
    private float size = 1f / Mathf.Sqrt(3f);

    /*
    void Awake()
    {
        for (int y = -mapLength; y <= mapLength; y++)
        {
            for (int x = -mapLength; x <= mapLength; x++)
            {
                Vector3 newPosition = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                GameObject tile = Instantiate(prefabTile, newPosition, this.transform.rotation, this.transform);
                tile.name = "Tile -> " + x + " " + y;
                //tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                //tile.transform.SetParent(this.transform);
            }
        }
    }
    */

    void Awake()
    {
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
                        Vector3 newPosition = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                        GameObject tile = Instantiate(prefabTile, newPosition, Quaternion.identity, this.transform);
                        tile.name = "Tile -> " + x + " " + y;
                        //tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                        //tile.transform.SetParent(this.transform);
                    }
                }
            }
        }
    }


    public Vector3Int OddToCube(Vector2Int axes)
    {
        int col = axes.x;
        int row = axes.y;
        int q = col - (row - (row & 1)) / 2;
        int r = row;
        return new Vector3Int(q, r, -q - r);
    }

    public Vector2Int CubeToOdd(Vector3Int qrs)
    {
        int q = qrs.x;
        int r = qrs.y;
        int col = q + (r - (r & 1)) / 2;
        int row = r;
        return new Vector2Int(col, row);
    }




    public Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
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
        float size = this.size;


        shouldOffset = (Math.Abs(row) % 2) == 1;
        height = 2f * size;
        width = Mathf.Sqrt(3f) * size;

        //width = this.tileWidth;
        //height = this.tileHeight;

        verticalDistance = height * (3f / 4f);
        horizontalDistance = width;

        offset = (shouldOffset) ? width / 2 : 0;
        xPosition = (column * horizontalDistance) + offset;
        yPosition = (row * verticalDistance);
        return new Vector3(xPosition, 0, -yPosition);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
