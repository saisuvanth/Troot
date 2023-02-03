using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighbourDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    private void Start(){
        foreach(Hex hex in FindObjectsOfType<Hex>()){
            hexTileDict.Add(hex.HexCoords, hex);
        }

        List<Vector3Int> neighbours = GetNeighboursFor(new Vector3Int(0,0,0));

        Debug.Log("Neighbours for (0,0,0): ");

        foreach(Vector3Int neighbourPos in neighbours){
            Debug.Log(neighbourPos);
        }

    }

    public Hex GetTileAt(Vector3Int hexCorrdinates){
        Hex result = null;
        hexTileDict.TryGetValue(hexCorrdinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates){
        
        if(hexTileDict.ContainsKey(hexCoordinates)==false)
            return new List<Vector3Int>();

        if(hexTileNeighbourDict.ContainsKey(hexCoordinates))
            return hexTileNeighbourDict[hexCoordinates];

        hexTileNeighbourDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach(Vector3Int direction in Direction.GetDirectionList(hexCoordinates.z)){
            
            if(hexTileDict.ContainsKey(hexCoordinates + direction)){
                hexTileNeighbourDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }

        return hexTileNeighbourDict[hexCoordinates];
    }
}


public static class Direction
{
    public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>()
    {
        new Vector3Int(-1,0,1), // N1
        new Vector3Int(0,0,1), // N2
        new Vector3Int(1,0,0), // E
        new Vector3Int(0,0,-1), // S2
        new Vector3Int(-1,0,-1), // S1
        new Vector3Int(-1,0,0), // W
    };

    public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>()
    {
        new Vector3Int(0,0,1), // N1
        new Vector3Int(1,0,1), // N2
        new Vector3Int(1,0,0), // E
        new Vector3Int(1,0,-1), // S2
        new Vector3Int(0,0,-1), // S1
        new Vector3Int(-1,0,0), // W
    };

    public static List<Vector3Int> GetDirectionList(int z) => z % 2 == 0 ? directionOffsetEven : directionOffsetOdd;
}