using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private ObjectLibrary _objectLibrary;

    public bool itemSpawner;
    private GameObject _myObject;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Init",1f);
    }

    public void Init()
    {
        _objectLibrary = FindObjectOfType<ObjectLibrary>();
        if (!itemSpawner)
        {
            _myObject = _objectLibrary.GetGameObject();
            if (_myObject != null)
            {
                Instantiate(_myObject, transform.position, transform.rotation);
            }
        }
        else
        {
            _myObject = _objectLibrary.GetItemObject();
            if (_myObject != null)
            {
                Instantiate(_myObject, transform.position, transform.rotation);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckObject()
    {
        
    }
}
