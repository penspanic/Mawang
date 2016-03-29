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
	}
	

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
}
