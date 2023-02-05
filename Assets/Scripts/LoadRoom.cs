using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadRoom : MonoBehaviour
{
    public TextMeshProUGUI roomId;
    void Start()
    {
        roomId.text = (RoomScript.joinedRoomData.Id+"0");
    }
}
