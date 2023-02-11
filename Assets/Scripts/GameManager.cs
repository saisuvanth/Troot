using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Coherence.Runtime;
using Coherence.Toolkit;

public class GameManager : MonoBehaviour
{
  public CoherenceMonoBridge bridge;
  private string P1, P2;

  public GameObject Ground;

  public Dictionary<Vector3Int, Tile> hexTileDict;
  public GameState gameState;
  public static string currentPlayer = "Player 2";
  public int P1score;
  public int P2score;
  public int maxScore;

  void Awake()
  {
    if (!MonoBridgeStore.TryGetBridge(gameObject.scene, out bridge))
    {
      return;
    }
    RoomData rm = RoomScript.joinedRoomData;
    Debug.Log(bridge);
    JoinRoom(rm);
  }

  void Start()
  {
    hexTileDict = Ground.GetComponent<GenerateMap>().populateDictionary();
    P1 = "Player 1";
    P2 = "Player 2";
    gameState = GameState.P1TURN;
  }

  public void onChange(Dictionary<Vector3Int, Tile> tileDict, GameState state)
  {
    hexTileDict = tileDict;
    gameState = state;
  }


  public void JoinRoom(RoomData roomData)
  {
    Debug.Log(bridge);
    try
    {
      bridge.JoinRoom(roomData);
      Debug.Log("Room Data: " + roomData);
      // Scene transition with room id
    }
    catch (System.Exception e)
    {
      Debug.Log("Error: " + e);
    }
  }

  public async void LeaveRoom()
  {
    // @saisuvanth @FreSauce check that the current user is the creator. If not, then don't allow them to leave the room.

    if (RoomScript.selectedRegion == "")
    {
      return;
    }

    try
    {
      Debug.Log("RoomScript.selectedRegion: " + RoomScript.selectedRegion);
      Debug.Log("RoomScript.joinedRoomData.Id: " + RoomScript.joinedRoomData.UniqueId);
      Debug.Log("RoomScript.joinedRoomData.Secret: " + RoomScript.joinedRoomData.Secret);
      await PlayResolver.RemoveRoom(RoomScript.selectedRegion, RoomScript.joinedRoomData.UniqueId, RoomScript.joinedRoomData.Secret);
    }
    catch (System.Exception e)
    {
      Debug.Log("Error: " + e);
    }

    UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
  }

  public void updateScore()
  {
    int mapLength = GenerateMap.mapLength;

    P1score = 0;
    P2score = 0;
    maxScore = 0;

    for (int q = -mapLength; q <= mapLength; q++)
    {
      for (int r = -mapLength; r <= mapLength; r++)
      {
        for (int s = -mapLength; s <= mapLength; s++)
        {
          if (q + r + s == 0)
          {
            if ((hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1OCCUPIED) || (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1DECAYED) || (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P1ROOT))
            {
              P1score++;
            }

            if ((hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2OCCUPIED) || (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2DECAYED) || (hexTileDict[new Vector3Int(q, r, s)].state == TileState.P2ROOT))
            {
              P2score++;
            }

            maxScore++;
          }

        }
      }
    }
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
      if (hexDict.ContainsKey(cubePoint + directions[i]))
      {
        neighbours[i] = hexDict[cubePoint + directions[i]];
      }
      else
      {
        neighbours[i] = null;
      }
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
