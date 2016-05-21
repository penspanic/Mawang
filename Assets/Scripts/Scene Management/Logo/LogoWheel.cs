using UnityEngine;

public class LogoWheel : MonoBehaviour
{
    public float stopPosX;
    public float rotateSpeed;
    public float moveSpeed;

    private bool isMoving = true;

    private void Awake()
    {
    }

    private void Update()
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