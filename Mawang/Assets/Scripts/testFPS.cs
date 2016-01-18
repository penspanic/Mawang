using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testFPS : MonoBehaviour 
{
    float deltaTime = 0.0f;
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        text.text = fps.ToString();
    }


}
