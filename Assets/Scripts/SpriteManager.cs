using UnityEngine;

public enum PackingType
{
    UI,
    Princess
}

public class SpriteManager : Singleton<SpriteManager>
{
    private Sprite[] uiPack;
    private Sprite[] princessPack;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        uiPack = Resources.LoadAll<Sprite>("UI");
        princessPack = Resources.LoadAll<Sprite>("PrincessPack");
    }

    public Sprite GetSprite(PackingType type, string sprName)
    {
        Sprite returnSpr = new Sprite();
        switch (type)
        {
            case PackingType.UI:
                returnSpr = FindSprite(uiPack, sprName);
                break;

            case PackingType.Princess:
                returnSpr = FindSprite(princessPack, sprName);
                break;

            default:
                break;
        }

        if (returnSpr == null)
            Debug.Log(sprName + " is none");

        return returnSpr;
    }

    private Sprite FindSprite(Sprite[] pack, string sprName)
    {
        return System.Array.Find(pack, (s) =>
         {
             return s.name == sprName;
         });
    }
}