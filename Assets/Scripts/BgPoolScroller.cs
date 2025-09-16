using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgPoolScroller : MonoBehaviour
{
    [SerializeField] private GameObject bgPrefab; // bg prefab to spawn and reuse
    [SerializeField] private int bgCount = 4;  // no of initial spawns 
    [SerializeField] private float spacing = 19.2f; // spacing between 2 prefabs
    [SerializeField] private float repositionXPos = -20f; // position where prefab respositions 

    public float speed;  // speed variable to be used by other scripts 
    private List<GameObject> scrollingObjects; // pool of objects
    private float totalWidth;  // total length of bg's combined

    string[] gateColors = {"red","green","blue"};

    void Start()
    {
        InitializeBG();
    }

    void Update()
    {
        RepositionBG();
    }

    /// <summary>
    /// initializes 4 bgs and adds to the pool 
    /// </summary>
    private void InitializeBG()
    {
        scrollingObjects = new List<GameObject>();
        totalWidth = bgCount * spacing;

        for (int i = 0; i < bgCount; i++)
        {
            Vector3 initialPosition = new Vector3(i * spacing, transform.position.y, transform.position.z);
            GameObject obj = Instantiate(bgPrefab, initialPosition, Quaternion.identity, transform);
            Gate gt = obj.transform.GetChild(0).GetComponent<Gate>();
            string selected = gateColors[Random.Range(0, 3)];
            gt.SetGate(selected);
            scrollingObjects.Add(obj);
        }
    }
    /// <summary>
    /// resposition the bg when out of bounds
    /// </summary>
    private void RepositionBG()
    {
        foreach (GameObject obj in scrollingObjects)
        {
            obj.transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            if (obj.transform.position.x < repositionXPos)
            {
                obj.transform.position += new Vector3(totalWidth, 0, 0);
                Gate gt = obj.transform.GetChild(0).GetComponent<Gate>();
                string selected = gateColors[Random.Range(0, 3)];
                gt.SetGate(selected);
            }
        }
    }

    /// <summary>
    /// Reset all Bg for restart
    /// </summary>
    public void ResetAllBg()
    {
        for(int i=0;i<scrollingObjects.Count;i++)
        {
            scrollingObjects[i].transform.position = new Vector3(19.1f*i, 0, 0);
        }
    }


}