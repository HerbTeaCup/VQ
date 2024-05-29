using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;

    [Header("Bullet Prefab")]
    [SerializeField] GameObject[] elementals = new GameObject[3];
    [SerializeField] Transform target;
    [SerializeField] Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        
        GameManager.Input.InputDelegate += IndexClamp;
        GameManager.Input.InputDelegate += StatusUpdate;
        GameManager.Input.InputDelegate += Fire;

        _status.AddForDestroy(this);
    }

    void Fire()
    {
        if (_status.fireCurrentRate > 0f)
        {
            _status.fireCurrentRate -= Time.deltaTime;
            if (_status.fireCurrentRate < 0.01f) { _status.fireCurrentRate = 0f; }
            return;
        }
        if (GameManager.Input.FireTrigger == false || GameManager.Input.Aiming == false) { return; }

        _status.fireCurrentRate = _status.fireRate;
        HS_ProjectileMover temp = Instantiate(elementals[GameManager.Input.Weapon_index], firePoint.position, this.transform.rotation).GetComponent<HS_ProjectileMover>();

        temp.dir = (target.transform.position - firePoint.position).normalized;
        temp = null;
    }
    void IndexClamp()
    {
        if(GameManager.Input.Weapon_index >= elementals.Length) { GameManager.Input.Weapon_index = 0; }
        else if(GameManager.Input.Weapon_index < 0) { GameManager.Input.Weapon_index = elementals.Length - 1; }
    }
    void StatusUpdate()
    {
        _status.type = (ElementType)GameManager.Input.Weapon_index;
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= IndexClamp;
        GameManager.Input.InputDelegate -= StatusUpdate;
        GameManager.Input.InputDelegate -= Fire;
    }
}
