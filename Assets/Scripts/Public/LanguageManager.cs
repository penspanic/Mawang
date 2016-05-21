using UnityEngine;

public enum Language
{
    Default,
    English,
}

public class LanguageManager : MonoBehaviour
{
    public Language currLanguage
    { get; private set; }

    private void Awake()
    {
    }

    private void Update()
    {
    }

    public string GetText(string key)
    {
        return null;
    }

    public void ChangeLanguage(string language)
    {
    }
}