using System.Collections.Generic;
using UnityEngine;

public enum GameEvent
{
    FirstC0S1Cleared, // 처음으로 C0S1 클리어 되었을 때
    FirstChapter0Cleared, // 처음으로 챕터 0 클리어 되었을 때
    AppRating
}

public class GameEventManager : MonoBehaviour
{
    #region Singleton

    private static GameEventManager _instance;

    public static GameEventManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newObj = new GameObject("Game Event Manager");
                _instance = newObj.AddComponent<GameEventManager>();
            }
            return _instance;
        }
    }

    #endregion Singleton

    private List<GameEvent> eventList = new List<GameEvent>();

    public void CheckInstance()
    {
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PushEvent(GameEvent gameEvent)
    {
        if (eventList.Contains(gameEvent))
            return;
        eventList.Add(gameEvent);
    }

    public bool EventExist(GameEvent gameEvent)
    {
        return eventList.Contains(gameEvent);
    }

    public void EventReceived(GameEvent gameEvent)
    {
        if (eventList.Contains(gameEvent))
            eventList.Remove(gameEvent);
    }
}