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

    private string selectedRegion;
    public static RoomData joinedRoomData;

    private async void Start()
    {
        if (!MonoBridgeStore.TryGetBridge(gameObject.scene, out bridge))
        {
            return; 
        }

        try{
            var regions = await PlayResolver.FetchRegions();
            dropdown.ClearOptions();

            Debug.Log("Regions: " + regions);

            foreach (var region in regions)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(region,null));
            }

            DropdownItemSelected(dropdown);

            dropdown.onValueChanged.AddListener(delegate {
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
        try{
            if(selectedRegion != "")
            {
                var creatorName=PlayerPrefs.GetString("Username");
                Debug.Log("Creating room in: " + selectedRegion);
                RoomCreationOptions options = new RoomCreationOptions();
                options.Tags = new string[] {creatorName};
                options.MaxClients = 2;
                var roomData=await PlayResolver.CreateRoom(selectedRegion,options);

                Debug.Log("Room Data: " + roomData);

                JoinRoom(roomData);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Error: " + e);
        }
    }

    public async void JoinSearchedRoom()
    {
        searchRoom(roomName.text);
    }

    public async void searchRoom(string roomName)
    {
        try{
            Debug.Log("Searching room in: " + roomName);
            string[] roomTags = {roomName};
            var roomData=await PlayResolver.FetchRooms(selectedRegion,roomTags);

            if(roomData.Count > 0)
            JoinRoom(roomData[0]);
            else
            roomErrorText.text = "Room not found";

        }
        catch (System.Exception e)
        {
            Debug.Log("Error: " + e);
        }
    }

    public async void JoinRoom(RoomData roomData)
    {
        try{
            bridge.JoinRoom(roomData);
            Debug.Log("Room Data: " + roomData);

            // Scene transition with room id
            joinedRoomData = roomData;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
        catch (System.Exception e)
        {
            Debug.Log("Error: " + e);
        }
    }
}
