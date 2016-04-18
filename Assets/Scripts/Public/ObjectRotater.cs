using UnityEngine;
using System.Collections;

public class ObjectRotater : MonoBehaviour
{

    public Vector3 rotateValue;
    public float rotateTime;
    public bool rotateWithScaledDeltaTime;


    void Awake()
    {

    }

    void Update()
    {
        if (rotateWithScaledDeltaTime)
            transform.Rotate(rotateValue * Time.deltaTime / rotateTime);
        else
            transform.Rotate(rotateValue * Time.unscaledDeltaTime / rotateTime);
    }
}
