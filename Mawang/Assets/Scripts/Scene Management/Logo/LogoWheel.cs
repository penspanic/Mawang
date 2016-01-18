using UnityEngine;
using System.Collections;

public class LogoWheel : MonoBehaviour
{
    public float stopPosX;
    public float rotateSpeed;
    public float moveSpeed;

    bool isMoving = true;
    void Awake()
    {

    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
            transform.Rotate(rotateSpeed * Vector2.left * Time.deltaTime);
            if (transform.position.x <= stopPosX)
            {
                isMoving = false;
            }
        }
    }
}
