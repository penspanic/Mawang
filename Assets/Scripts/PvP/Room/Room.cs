using System;
using System.Net;
using System.Net.Sockets;

public class Room
{
    private TcpListener tcpListener;

    public Room()
    {
    }

    public void Init()
    {
        tcpListener = new TcpListener(IPAddress.Any, 9810);
        //tcpListener.BeginAcceptSocket(OnAccept)
    }

    private void OnAccept(IAsyncResult ar)
    {
    }

    private void NotifyState()
    {
    }
}