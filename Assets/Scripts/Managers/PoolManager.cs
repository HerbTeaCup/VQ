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
        Root = new GameObject().transform;  //��Ʈ ����
        Root.name = $"{objDictionary[name].name}_Root";   //��Ʈ�� �̸��� Pooling�� ���������� ��Ʈ

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
        poolQueue.Add(name, new Queue<GameObject>());//���� ť �Ҵ�
        objDictionary.Add(name, obj);//�߰����� Ǯ�� ���
    }
    public void Enqueue(string name, GameObject obj)
    {
        Poolable temp = null;
        if (temp.TryGetComponent<Poolable>(out temp) == false) { Debug.LogWarning("Pooling �Ұ� ������Ʈ Pooling��"); return; }

        if (poolQueue.ContainsKey(name) == false)
        {
            Add(name, obj);
            Create(name, 3);//�⺻������ 3�� �߰�
            return;
        }

        //�̹� �ִ� �ָ� �׳� ��ȯ
        obj.SetActive(false);
        poolQueue[name].Enqueue(obj);
    }
    public GameObject Dequeue(string name)
    {
        if (poolQueue.ContainsKey(name) == false)
        {
            Debug.LogWarning($"���� name�� PoolQueue�� �������� ���� (Pop �Ұ�).\nWarning for {name}");
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
