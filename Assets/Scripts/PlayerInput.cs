using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Coherence;
using Coherence.Toolkit;

public class PlayerInput : MonoBehaviour
{
  public new Camera camera;

  private CoherenceSync coherenceSync;
  private GameManager gameManager;

  private Vector3 _prevMousePos;

  void Start()
  {
    coherenceSync = GetComponent<CoherenceSync>();
    gameManager = GetComponent<GameManager>();
  }

  void Update()
  {
    DetectClick();
  }

  private void DetectClick()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 mousPos = Input.mousePosition;
      Ray ray = camera.ScreenPointToRay(mousPos);
      ray.origin = camera.transform.position;
      Debug.Log("click detected");
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, Mathf.Infinity))
      {
        Debug.DrawLine(ray.origin, hit.point, Color.red, 10f);
        float posX = hit.point.x;
        float posZ = hit.point.z;
        Vector3Int cubePoint = GenerateMap.OddToCube(GenerateMap.WorldToOdd(hit.point));
        Vector3 cubePointV3 = new Vector3(cubePoint.x, cubePoint.y, cubePoint.z);
        coherenceSync.SendCommand<PlayerInput>(nameof(PlayerInput.onClick), MessageTarget.AuthorityOnly, cubePointV3, ray.origin, hit.point);
      }
    }
  }

  public void onClick(Vector3 cubePointV3, Vector3 start, Vector3 end)
  {
    Debug.DrawLine(start, end, Color.red, 10f);
    Vector3Int cubePoint = new Vector3Int((int)cubePointV3.x, (int)cubePointV3.y, (int)cubePointV3.z);
    Debug.Log("clicked on other client " + cubePoint);
    GameState state = gameManager.gameState;
    Debug.Log("dic state: " + gameManager.hexTileDict[cubePoint].state);
    if (gameManager.hexTileDict.ContainsKey(cubePoint))
    {
      if (gameManager.hexTileDict[cubePoint].state == TileState.EMPTY)
      {
        Tile[] nTiles = gameManager.hexTileDict[cubePoint].GetNeighbours(gameManager.hexTileDict, cubePoint);
        Debug.Log("nTiles length: " + nTiles.Length);
        foreach (var tile in nTiles)
        {
          if (tile != null && (tile.state == (state == GameState.P1TURN ? TileState.P1OCCUPIED : TileState.P2OCCUPIED) || tile.state == (state == GameState.P1TURN ? TileState.P1ROOT : TileState.P2ROOT)))
          {
            gameManager.hexTileDict[cubePoint].state = state == GameState.P1TURN ? TileState.P1OCCUPIED : TileState.P2OCCUPIED;
            gameManager.gameState = state == GameState.P1TURN ? GameState.P2TURN : GameState.P1TURN;
            tile.transform.GetComponent<TileData>().tileUpdate(gameManager.hexTileDict);
          }
        }
        GameObject obj = GameObject.Find(string.Format("Tile -> {0} {1} {2}", cubePoint.x, cubePoint.y, cubePoint.z));
        obj.GetComponent<TileData>().tileUpdate(gameManager.hexTileDict);
        // gameManager.updateScore();
      }
    }
  }
}
