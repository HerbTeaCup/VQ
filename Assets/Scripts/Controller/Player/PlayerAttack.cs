using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;

    int prefabIndex = 0;

    [Header("Bullet Prefab")]
    [SerializeField] GameObject[] elementals = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
    }

    public void IndexUp()
    {
        prefabIndex++;
        if(prefabIndex >= elementals.Length) { prefabIndex = 0; }
    }
    public void IndexDown()
    {
        prefabIndex--;
        if(prefabIndex < 0) { prefabIndex = elementals.Length - 1; }
    }
}
