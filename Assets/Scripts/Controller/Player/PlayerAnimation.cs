using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;
    Animator _anim;

    bool _attackable = true; //애니메이션 중 코루틴 중복 방지

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _anim = GetComponent<Animator>();

        _anim.applyRootMotion = false;

        GameManager.Input.InputDelegate += ParameterUpdate;

        _status.AddForDestroy(this);
    }

    void ParameterUpdate()
    {
        _anim.SetFloat("SpeedBlend", _status.currentSpeed);
        _anim.SetFloat("AnimationSpeed", _status.animationSpeed);

        if(GameManager.Input.FireTrigger == true && _attackable) 
        {
            _status.currentSpeed = 0f;
            _anim.SetTrigger("AttackTrigger"); StartCoroutine("MoveControlCoroutine", 1f); 
        }
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= ParameterUpdate;
    }

    IEnumerator MoveControlCoroutine(float time)
    {
        _status.isMoveable = false;
        _attackable = false;
        yield return new WaitForSeconds(time / _status.animationSpeed);
        _status.isMoveable = true;
        _attackable = true;
    }
}
