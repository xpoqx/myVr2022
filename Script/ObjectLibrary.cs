using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction.Input;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectLibrary : MonoBehaviour
{
    public static ObjectLibrary Instance;
    public GameObject[] gameObjects,goToFind;
    public GameObject[] goList1,goList2,goList3, goFindList1;
    public Queue<GameObject> GOs;
    public GameObject[] crystals;
    public GameObject[] items;
    public List<int> usedIndex;
    public int spawnCount;

    public bool randomSwitch;

    private int _idx;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);
        spawnCount = 0;

        randomSwitch = false;
        _idx = 0;
        
        GOs = new Queue<GameObject>();
        EnQueueGameObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnQueueGameObjects()
    {
        var goList = gameObjects.OrderBy(a => Guid.NewGuid()).ToList();
        for (int i = 0; i < goList.Count; i++)
        {
            GOs.Enqueue(goList[i]);
            Debug.Log("Enqueueing..");
        }
    }

    public GameObject GetGameObject()
    {
        if (GOs.Count > 0) return GOs.Dequeue();
        else return null;
    }
    
    public GameObject GetItemObject()
    {
        var a = Random.Range(0, items.Length);
        return items[a];
    }
}
