using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRise : MonoBehaviour
{
    public List<GameObject> platforms;
    public float yOffset;
    public float speedOfRising;

    private List<float> initialYPositions = new List<float>();

    void Start()
    {
        if (platforms == null || platforms.Count <= 0)
        {
            UtilityScript.LogError("This " + gameObject.name + " Path of " + gameObject.transform.parent.name + " has no Path Objects Assigned!");
            return;
        }
        InitialStuff();
    }
    private void InitialStuff()
    {

        initialYPositions = new List<float>();
        foreach (GameObject obj in platforms)
        {
            initialYPositions.Add(obj.transform.position.y);
        }
    }
    public Coroutine MovePlatformsUp()
    {
        return StartCoroutine(MovePlatformsCoroutine());
    }

    private IEnumerator MovePlatformsCoroutine()
    {
        foreach (GameObject obj in platforms)
        {
            Vector3 newPosition = obj.transform.position;
            newPosition.y -= yOffset;
            obj.transform.position = newPosition;
        }

        for (int i = 0; i < platforms.Count; i++)
        {
            GameObject obj = platforms[i];
            obj.SetActive(true);
            yield return StartCoroutine(MoveToInitialPosition(obj, initialYPositions[i]));
        }
    }

    private IEnumerator MoveToInitialPosition(GameObject obj, float targetY)
    {
        while (Mathf.Abs(obj.transform.position.y - targetY) > 0.01f)
        {
            Vector3 newPosition = obj.transform.position;
            newPosition.y = Mathf.MoveTowards(newPosition.y, targetY, speedOfRising * Time.deltaTime);
            obj.transform.position = newPosition;
            yield return null;
        }
    }

    public void ChangePathVisibility(bool shouldEnable)
    {
        foreach (GameObject obj in platforms)
        {
            obj.SetActive(shouldEnable);
        }
    }
}
