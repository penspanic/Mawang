using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    private List<Room> roomList = new List<Room>();

    private Socket client;
    private Socket host;

    private IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5670);
    private byte[] receiveBuffer = new byte[1024];

    private void Awake()
    {
    }

    public void ConnectRoom()
    {
        try
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(ipEndPoint, new AsyncCallback(Connected), client);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
            CreateRoom();
        }
    }

    private void Connected(IAsyncResult ar)
    {
        host = (Socket)ar.AsyncState;
        Debug.Log(host.RemoteEndPoint.ToString() + "에 연결 완료");
        Debug.Log(ar.ToString());
        client.BeginReceive(
            receiveBuffer, 0, receiveBuffer.Length,
            SocketFlags.Broadcast, new AsyncCallback(Receive), client
            );
    }

    private void Receive(IAsyncResult ar)
    {
        int byteSize = client.EndReceive(ar);

        Debug.Log(Encoding.UTF8.GetString(receiveBuffer, 0, byteSize));
        client.BeginReceive(
            receiveBuffer, 0, receiveBuffer.Length,
            SocketFlags.Broadcast, new AsyncCallback(Receive), client
            );
    }

    public void SendBytes(byte[] bytes)
    {
        client.Send(bytes);
    }

    private Room CreateRoom()
    {
        Debug.Log("방 생성중...");
        return null;
    }

    public bool HasRoom()
    {
        return roomList.Count > 0;
    }
}