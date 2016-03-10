using UnityEngine;
using System.Collections;

/// <summary>
///   < 카메라 무빙 >
/// 1. 카메라 움직이는거
///   < 추가요구사항 >
/// 1. 카메라가 자연스럽게 움직이는거 ( http://sonicclass.tistory.com/79 )
/// </summary>

public class TestMovingCam : MonoBehaviour
{

    #region GameDesign

    [SerializeField]
    private float speed;
    public float pcSpeed = 5;
    #endregion

    private Transform camTransform;
    private Vector2 wp;
    private Ray2D ray;
    private RaycastHit2D hit;

    void Awake()
    {
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            if(!TutorialManager.Instance.isPlaying)
                return;
        }

        if (TutorialManager.Instance.isPlaying && !TutorialManager.Instance.camMove)
            return;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0))
                    return;

                wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                // 레이2d를 갖고옴
                ray = new Ray2D(wp, Vector2.zero);
                // 레이캐스트를 쏨
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider == null || hit.collider.tag != "UI")
                {
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                    transform.Translate(-touchDeltaPosition.x * speed, 0, 0);
                    transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -1.4f, 12.8f),
                        transform.localPosition.y, transform.localPosition.z);
                }
            }
            // 끝날때 
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-pcSpeed * speed, 0, 0);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -1.4f, 12.8f),
                    transform.localPosition.y, transform.localPosition.z);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(pcSpeed * speed, 0, 0);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -1.4f, 12.8f),
                    transform.localPosition.y, transform.localPosition.z);
            }
        }

    }

}
