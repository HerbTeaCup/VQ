using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    public T Load<T>(string path) where T : Object
    {
        T temp = Resources.Load<T>(path);
        return temp;
    }
    public GameObject Instantiate(GameObject obj, Transform parent = null)
    {
        Poolable temp;
        if (obj.TryGetComponent<Poolable>(out temp))
        {
            return Object.Instantiate(GameManager.Pool.Dequeue(obj.name), parent);
        }
        else
        {
            return Object.Instantiate(obj, parent);
        }
    }
}
