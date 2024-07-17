using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;

    [Header("Bullet Prefab")]
    [SerializeField] GameObject[] elementals;
    [SerializeField] Transform target;
    [SerializeField] Transform firePoint;

    int[] _magazine;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _magazine = new int[elementals.Length];
        
        GameManager.Input.InputDelegate += IndexClamp;
        GameManager.Input.InputDelegate += StatusUpdate;
        GameManager.Input.InputDelegate += Fire;

        _status.AddForDestroy(this);
    }

    void Fire()
    {
        if (_status.fireCurrentRate > 0f)
        {
            _status.attackable = false;
            _status.fireCurrentRate -= Time.deltaTime;
            if (_status.fireCurrentRate < 0.01f) { _status.fireCurrentRate = 0f; }
            return;
        }

        _status.attackable = true;
        if (GameManager.Input.FireTrigger == false || GameManager.Input.Aiming == false || _status.isCarrying) { return; }

        _status.fireCurrentRate = _status.fireRate;
        HS_ProjectileMover temp = Instantiate(elementals[GameManager.Input.Weapon_index], firePoint.position, this.transform.rotation).GetComponent<HS_ProjectileMover>();

        //�� �ڵ� ����� �������θ� �������� ��Ÿ��� ������ ���� �־���
        //target.position = new Vector3(target.position.x, firePoint.position.y, target.position.z);

        temp.dir = (target.position - firePoint.position).normalized;
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
