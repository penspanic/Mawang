using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;

public class TestCode : MonoBehaviour, IPointerDownHandler
{
    public Image test;


    Texture2D spriteTexture;
	// Use this for initialization
	void Start () 
    {
        spriteTexture = test.sprite.texture;

        Debug.Log("Color : " + spriteTexture.GetPixel(100, 200));
	}
	

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Debug.Log("COLOR : " + spriteTexture.GetPixel((int)(1280f * mousePos.x), (int)(720f * mousePos.y)));
    }
}
