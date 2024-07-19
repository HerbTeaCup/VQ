using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationAndBehavior : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;
    Animator _anim;

    [SerializeField] GameObject seed;
    [SerializeField] GameObject PlayerVirtualSeed;

    float _radius = 0.5f;

    bool _hasItem = false;

    Collider[] items;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _anim = GetComponent<Animator>();

        _anim.applyRootMotion = false;

        GameManager.Input.InputDelegate += ParameterUpdate;
        GameManager.Input.InputDelegate += Founding;
        GameManager.Input.InputDelegate += CheckCliff;

        _status.AddForDestroy(this);
    }
    void ParameterUpdate()
    {
        SpeedParameter();
        AttackParameter();
        InteractiveParameter();
    }
    void SpeedParameter()
    {
        _anim.SetFloat("SpeedBlend", _status.currentSpeed);
        _anim.SetFloat("AnimationSpeed", _status.animationSpeed);
    }
    void AttackParameter()
    {
        if (GameManager.Input.FireTrigger == true && _status.attackable && GameManager.Input.Aiming)
        {
            _status.currentSpeed = 0f;
            _anim.SetTrigger("AttackTrigger"); 
            StartCoroutine("MoveControlCoroutine", 1f);
        }
    }
    void InteractiveParameter()
    {
        if (GameManager.Input.Interactive == false || _status.behaviourable == false) { return; }

        if (_status.isCarrying)
        {
            if (_hasItem || _status.isClifAhead) { return; }
            _anim.SetTrigger("Lift");
            _status.isCarrying = false;
            StartCoroutine(Waiting(0.7f));
            return;
        }

        if (_hasItem)
        {
            InteracitveObjType type = items[0].GetComponent<InteractableClass>().type;
            switch (type)
            {
                case InteracitveObjType.Seed:
                    _status.isCarrying = true;
                    _anim.SetTrigger("Lift");
                    StartCoroutine(Waiting());
                    break;
                case InteracitveObjType.Box:
                    Debug.Log("Box Interaction Logic");
                    StartCoroutine(Waiting());
                    break;
            }
        }
    }

    void Founding()
    {
        items = Physics.OverlapSphere(this.transform.position, _radius, 1 << 7);// 1<<7 == Intereaction Layer
        _hasItem = items.Length > 0;
    }
    void CheckCliff()
    {
        Vector3 startPos = this.transform.position + transform.forward * _radius;

        //0.4f는 계단에서 문제가 생길 수도 있음.
        bool frontCliff = !Physics.Raycast(startPos, Vector3.down, 0.4f);

        _status.isClifAhead = frontCliff;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _hasItem ? Color.green : Color.red;

        Gizmos.DrawWireSphere(this.transform.position, _radius);
    }

    IEnumerator Waiting(float duration = 0.9f)
    {
        _status.behaviourable = false;
        _status.isMoveable = false;
        yield return new WaitForSeconds(duration / _status.animationSpeed);
        _status.behaviourable = true;
        _status.isMoveable = true;
    }
    IEnumerator MoveControlCoroutine(float time)
    {
        _status.isMoveable = false;
        yield return new WaitForSeconds(time / _status.animationSpeed);
        _status.isMoveable = true;
    }

    //Animation Event
    public void InteractiveEvent()
    {
        IPuzzleInteraction temp = items[0].GetComponent<IPuzzleInteraction>();
        temp.Interactive();
    }
    public void LiftDownEvent()
    {
        Instantiate(seed, this.transform.position + transform.forward * _radius, Quaternion.identity);
        PlayerVirtualSeed.SetActive(false);
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= ParameterUpdate;
    }
}
