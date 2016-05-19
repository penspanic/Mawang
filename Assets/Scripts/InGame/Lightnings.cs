using UnityEngine;
using System.Collections;

public class Lightnings : MonoBehaviour
{
    public GameObject lightningPrefab;
    public Vector2 leftTop;
    public Vector2 rightBottom;

    ObjectPool<GameObject> lightningPool;
    void Awake()
    {
        lightningPool = new ObjectPool<GameObject>(10, lightningPrefab, (GameObject original) =>
        {
            GameObject newLightning = Instantiate(original);
            newLightning.SetActive(false);
            return newLightning;
        });
        StartCoroutine(LightningProcess());
    }

    IEnumerator LightningProcess()
    {
        GameObject currLightning = null;
        while (true)
        {
            float nextTime = Random.Range(1, 2);
            yield return new WaitForSeconds(nextTime);
            currLightning = lightningPool.pop();
            currLightning.SetActive(true);
            Vector2 newPos = new Vector2();
            newPos.x = Random.Range(leftTop.x, rightBottom.x);
            newPos.y = Random.Range(rightBottom.y, leftTop.y);
            currLightning.transform.position = newPos;
            yield return new WaitForSeconds(1f);
            currLightning.SetActive(false);
            lightningPool.push(currLightning);
        }
    }
}
