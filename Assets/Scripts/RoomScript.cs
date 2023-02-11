using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coherence.Runtime;
using Coherence.Toolkit;
using TMPro;

public class RoomScript : MonoBehaviour
{
  private CoherenceMonoBridge bridge;
  public TMP_Dropdown dropdown;
  public TextMeshProUGUI TextBox;
  public TMP_InputField roomName;
  public TextMeshProUGUI roomErrorText;

  public static string selectedRegion;
  public static RoomData joinedRoomData;

  public void Start()
  {
    RoomStart();
  }

  private async void RoomStart()
  {
    if (!MonoBridgeStore.TryGetBridge(gameObject.scene, out bridge))
    {
      return;
    }

    try
    {
      var regions = await PlayResolver.FetchRegions();
      dropdown.ClearOptions();

      Debug.Log("Regions: " + regions);

      foreach (var region in regions)
      {
        dropdown.options.Add(new TMP_Dropdown.OptionData(region, null));
      }

      DropdownItemSelected(dropdown);

      dropdown.onValueChanged.AddListener(delegate
      {
        DropdownItemSelected(dropdown);
      });
    }
    catch (System.Exception e)
    {
      Debug.Log("Error: " + e);
    }
  }

  void DropdownItemSelected(TMP_Dropdown dropdown)
  {
    int index = dropdown.value;
    selectedRegion = dropdown.options[index].text;
    TextBox.text = dropdown.options[index].text;
  }

  public async void CreateRoom()
  {
    try
    {
      if (selectedRegion != "")
      {
        var creatorName = PlayerPrefs.GetString("Username");
        Debug.Log(selectedRegion);
        RoomCreationOptions options = RoomCreationOptions.Default;
        options = RoomCreationOptions.Default;
        options.KeyValues = new Dictionary<string, string> { { RoomData.RoomNameKey, creatorName } };
        options.MaxClients = 2;
        options.Tags = new string[] { creatorName };
        var roomData = await PlayResolver.CreateRoom(selectedRegion, options);

        joinedRoomData = roomData;
        Debug.Log("Room Data: " + roomData);

        GameManager.currentPlayer = "Player 1";

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        // JoinRoom(roomData);
      }
    }
    catch (System.Exception e)
    {
      Debug.Log("Error: " + e);
    }
  }

  public void JoinSearchedRoom()
  {
    searchRoom(roomName.text);
  }

  public async void searchRoom(string roomName)
  {
    try
    {
      Debug.Log("Searching room in: " + roomName);
      string[] roomTags = { roomName };
      var roomData = await PlayResolver.FetchRooms(selectedRegion, roomTags);

      if (roomData.Count > 0)
      {

        Debug.Log("Room Data: " + roomData[0]);
        joinedRoomData = roomData[0];
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        // JoinRoom(roomData[0]);
      }
      else
        roomErrorText.text = "Room not found";

    }
    catch (System.Exception e)
    {
      Debug.Log("Error: " + e);
    }
  }


}
