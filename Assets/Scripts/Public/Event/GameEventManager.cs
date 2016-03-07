using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameEvent
{
    FirstC0S1Cleared, // 처음으로 C0S1 클리어 되었을 때
    FirstChapter0Cleared, // 처음으로 챕터 0 클리어 되었을 때
}
public class GameEventManager : MonoBehaviour
{
    #region Singleton
    static GameEventManager _instance;

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
    #endregion

    List<GameEvent> eventList = new List<GameEvent>();
    
    public void CheckInstance()
    {

    }

    void Awake()
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