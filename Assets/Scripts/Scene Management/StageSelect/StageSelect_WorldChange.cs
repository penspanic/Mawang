using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect_WorldChange : MonoBehaviour
{
    public int currWorldIndex
    { get; private set; }

    public GameObject[] worlds;

    private bool changing = false;
    private StageSelect_PinchZoom pinchZoom;

    private void Awake()
    {
        pinchZoom = GameObject.FindObjectOfType<StageSelect_PinchZoom>();
    }

    public void OnButtonDown()
    {
        if (changing)
            return;
        StartCoroutine(ChangeWorld());
    }

    private IEnumerator ChangeWorld()
    {
        changing = true;

        float elapsedTIme = 0f;
        const float changeTime = 1f;
        Vector2 endPos = new Vector2(6.4f, 25f) * pinchZoom.ratio;
        Vector2 startPos = new Vector2(6.4f, 3.6f);
        Vector3 endRotation = new Vector3(0, 0, 90);

        int newWorldIndex = currWorldIndex == 0 ? 1 : 0;
        worlds[newWorldIndex].SetActive(true);
        worlds[currWorldIndex].transform.SetSiblingIndex(worlds.Length - 1);
        Image currWorldImage = worlds[currWorldIndex].GetComponent<Image>();

        while (elapsedTIme < changeTime)
        {
            elapsedTIme += Time.deltaTime;
            worlds[currWorldIndex].transform.position = EasingUtil.EaseVector3(EasingUtil.easeInOutCubic, startPos, endPos, elapsedTIme / changeTime);
            worlds[currWorldIndex].transform.rotation = Quaternion.Euler(new Vector3(0, 0, EasingUtil.easeInOutCubic(0, endRotation.z, elapsedTIme / changeTime)));
            currWorldImage.color = new Color(1, 1, 1, 1 - EasingUtil.easeInOutCubic(0, 1, elapsedTIme / changeTime));
            yield return null;
        }
        worlds[currWorldIndex].transform.position = startPos;
        worlds[currWorldIndex].transform.rotation = new Quaternion();
        currWorldImage.color = new Color(1, 1, 1, 1);
        worlds[currWorldIndex].SetActive(false);

        currWorldIndex = newWorldIndex;

        changing = false;
    }
}