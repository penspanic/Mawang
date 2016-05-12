using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : Singleton<NetworkManager>
{
    private List<Room> roomList = new List<Room>();

    void Awake()
    {

    }

    void SearchRoom()
    {

    }

    public Room CreateRoom()
    {
        //Room newRoom;
        //roomList.Add(newRoom);
        return null;
    }

    public bool HasRoom()
    {
        return roomList.Count > 0;
    }

    public void ConnectRoom()
    {

    }
}
