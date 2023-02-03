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
    void Awake()
    {
        for (int x = 0; x < mapLength; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 newPosition = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                GameObject tile = Instantiate(prefabTile, newPosition, this.transform.rotation, this.transform);
                tile.name = "Tile -> " + x + " " + y;
                //tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));
                //tile.transform.SetParent(this.transform);
            }
        }

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

        shouldOffset = (row % 2) == 0;
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
