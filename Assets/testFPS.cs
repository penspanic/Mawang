using UnityEngine;
using UnityEngine.UI;

public class testFPS : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        text.text = fps.ToString();
    }
}