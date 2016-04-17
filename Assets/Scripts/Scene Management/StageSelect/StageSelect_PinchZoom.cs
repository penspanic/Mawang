using UnityEngine;
using System.Collections.Generic;

public class StageSelect_PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed;     // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed;           // The rate of change of the orthographic size in orthographic mode.

    public float ratio
    {
        get;
        private set;
    }

    Vector3 cameraOriginalScale;
    void Awake()
    {
        cameraOriginalScale = Camera.main.transform.localScale;
    }

    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

            // Make sure the orthographic size never drops below zero.
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 3.6f);

            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, 7.2f);
        }

        ratio = Camera.main.orthographicSize / 3.6f;
        Camera.main.transform.localScale = ratio * cameraOriginalScale;
    }
}