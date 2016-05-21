using UnityEngine;

public class GameRoom : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.instance.ConnectRoom();
    }
}