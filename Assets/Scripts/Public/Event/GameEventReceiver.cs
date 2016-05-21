public class GameEventReceiver
{
    public event System.Action actionEvent;

    private GameEvent targetEvent;

    public GameEventReceiver(GameEvent targetEvent, System.Action action = null)
    {
        this.targetEvent = targetEvent;
        if (action != null)
            actionEvent += action;
    }

    public void CheckEvent()
    {
        if (GameEventManager.instance.EventExist(targetEvent))
        {
            GameEventManager.instance.EventReceived(targetEvent);
            if (actionEvent != null)
            {
                actionEvent();
            }
        }
    }
}