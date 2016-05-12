using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public class Room
{
    TcpListener tcpListener;

    public Room()
    {
       
    }

    public void Init()
    {
        tcpListener = new TcpListener(IPAddress.Any, 9810);
        //tcpListener.BeginAcceptSocket(OnAccept)
    }

    void OnAccept(IAsyncResult ar)
    {

    }

    void NotifyState()
    {

    }
}
