using UnityEngine;
using System.Collections;

public class GameRoom : MonoBehaviour
{

    void Awake()
    {
        NetworkManager.instance.ConnectRoom();
    }
}
