using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;
    CharacterController _cc;

    float _speed = 0f;
    float _gravity = -9.8f;
    float _verticalSpeed = 0f;

    Transform _point;
    Vector3 inputdir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        GetPoint();

        _cc = GetComponent<CharacterController>();
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += Move;
        GameManager.Input.InputDelegate += Gravity;
        GameManager.Input.InputDelegate += Rotate;

        _status.AddForDestroy(this);
    }

    void GetPoint()
    {
        if (_point != null) { return; }

        foreach (Transform child in this.transform)
        {
            if (child.name == "MousePoint")
            {
                _point = child;
                return;
            }
        }
    }

    void Move()
    {
        if(_status.isMoveable == false || GameManager.Input.Aiming) { return; }
        float targetSpeed = GameManager.Input.Sprint ? _status.runSpeed : _status.walkSpeed;

        if (GameManager.Input.XZdir == Vector2.zero)
        {
            targetSpeed = 0f;
        }

        float speedOffset = 0.1f;

        if (_status.currentSpeed < targetSpeed - speedOffset || _status.currentSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(_status.currentSpeed, targetSpeed, _status.speedRatio * Time.deltaTime);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        inputdir.x = GameManager.Input.XZdir.x;
        inputdir.z = GameManager.Input.XZdir.y;//�̹� normalized �Ǿ�����.

        _cc.Move(inputdir * _speed * Time.deltaTime + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        _status.currentSpeed = _speed;
    }
    void Rotate()
    {
        if(_status.isMoveable == false) { return; }

        if (GameManager.Input.Aiming)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_point.position), _status.rotateRatio * Time.deltaTime);
            return;
        }

        if (inputdir == Vector3.zero) { return; }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(inputdir), _status.rotateRatio * Time.deltaTime);
    }

    void Gravity()
    {
        if (_status.isGrounded)
        {
            if (_verticalSpeed < 0f)
            {
                _verticalSpeed = -2f;
            }
        }
        else
        {
            _verticalSpeed += _gravity * Time.deltaTime;
        }
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Move;
        GameManager.Input.InputDelegate -= Gravity;
        GameManager.Input.InputDelegate -= Rotate;
    }
}
