using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public SoundType type;

    public enum SoundType
    {
        BasicSound,
        BackSound
    }

    private AudioSource source;
    private Button button;

    private static AudioClip backSound;
    private static AudioClip basicSound;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);

        if (backSound == null || basicSound == null)
        {
            backSound = Resources.Load<AudioClip>("Sound/Effect/BackClick");
            basicSound = Resources.Load<AudioClip>("Sound/Effect/BasicClick");
        }

        switch (type)
        {
            case SoundType.BasicSound:
                source.clip = basicSound;
                break;

            case SoundType.BackSound:
                source.clip = backSound;
                break;
        }
    }

    private void OnClick()
    {
        source.Play();
    }

    public static void PlaySound(SoundType type)
    {
        AudioClip sound = type == SoundType.BasicSound ? basicSound : backSound;
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(sound);
    }
}