using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;
    Animator _anim;

    bool _attackable = true;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _anim = GetComponent<Animator>();

        GameManager.Input.InputDelegate += ParameterUpdate;
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
        yield return new WaitForSeconds(time / _status.animationSpeed); // 여기서 2f는 공격 애니메이션의 시간
        _status.isMoveable = true;
        _attackable = true;
    }
}
