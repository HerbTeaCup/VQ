using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : IManager
{
    Dictionary<string, Queue<GameObject>> poolQueue = new Dictionary<string, Queue<GameObject>>();
    Dictionary<string, GameObject> objDictionary = new Dictionary<string, GameObject>();

    Transform Root;
    void Create(string name, int cnt)
    {
        Root = new GameObject().transform;  //루트 생성
        Root.name = $"{objDictionary[name].name}_Root";   //루트의 이름은 Pooling할 오리지널의 루트

        for (int i = 0; i < cnt; i++)
        {
            GameObject temp = Object.Instantiate(objDictionary[name]);
            temp.SetActive(false);
            temp.transform.SetParent(Root);
            poolQueue[name].Enqueue(temp);
        }
    }

    void Add(string name, GameObject obj)
    {
        poolQueue.Add(name, new Queue<GameObject>());//동적 큐 할당
        objDictionary.Add(name, obj);//추가적인 풀링 대비
    }
    public void Enqueue(string name, GameObject obj)
    {
        Poolable temp = null;
        if (temp.TryGetComponent<Poolable>(out temp) == false) { Debug.LogWarning("Pooling 불가 오브젝트 Pooling중"); return; }

        if (poolQueue.ContainsKey(name) == false)
        {
            Add(name, obj);
            Create(name, 3);//기본적으로 3개 추가
            return;
        }

        //이미 있는 애면 그냥 반환
        obj.SetActive(false);
        poolQueue[name].Enqueue(obj);
    }
    public GameObject Dequeue(string name)
    {
        if (poolQueue.ContainsKey(name) == false)
        {
            Debug.LogWarning($"같은 name의 PoolQueue가 존재하지 않음 (Pop 불가).\nWarning for {name}");
            return null;
        }

        if (poolQueue[name].Count == 0)
        {
            Create(name, 3);
        }

        GameObject obj = poolQueue[name].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Clear()
    {
        poolQueue.Clear();
    }
}
