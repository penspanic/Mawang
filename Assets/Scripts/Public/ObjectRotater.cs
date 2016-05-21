using UnityEngine;

public class ObjectRotater : MonoBehaviour
{
    public Vector3 rotateValue;
    public float rotateTime;
    public bool rotateWithScaledDeltaTime;

    private void Awake()
    {
    }

    private void Update()
    {
        if (rotateWithScaledDeltaTime)
            transform.Rotate(rotateValue * Time.deltaTime / rotateTime);
        else
            transform.Rotate(rotateValue * Time.unscaledDeltaTime / rotateTime);
    }
}