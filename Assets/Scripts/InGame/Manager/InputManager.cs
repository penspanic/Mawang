using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>

public class InputManager : MonoBehaviour
{
    private SelectTab selectTab;
    private Vector2 wp;
    private Ray2D ray;
    private RaycastHit2D[] hit;
    void Awake()
    {
        selectTab   =   FindObjectOfType<SelectTab>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
                return;

            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 레이캐스트 사용
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(touchPos, Vector2.zero);
            for (int i = 0; i < hitInfo.Length; i++)
            {
                if (hitInfo[i].collider != null)
                {
                    GameObject hitObject = hitInfo[i].collider.gameObject;
                    if (hitObject.GetComponent<ITouchable>() != null)
                    {
                        // 오브젝트가 아군이나 적군일때 ( 성 포함 )
                        if (hitObject.CompareTag("OurForce") && selectTab.isSelected)
                            return;

                        hitObject.GetComponent<ITouchable>().OnTouch();
                    }
                }
                // 맞은 오브젝트에 터치 함수 호출
            }
        }
        if (Input.touchCount > 0 && Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SendMessage("ClickedButton");
            }
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0))
                return;
            wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);


            hit = Physics2D.RaycastAll(wp, Vector2.zero);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider != null)
                {
                    GameObject hitObj = hit[i].collider.gameObject;
                    if (hitObj.GetComponent<ITouchable>() != null)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            // 오브젝트가 아군이나 적군일때 ( 성 포함 )
                            if (hitObj.CompareTag("OurForce") && selectTab.isSelected)
                                return;

                            hitObj.GetComponent<ITouchable>().OnTouch();
                        }
                    }
                }
            }
        }

    }


}