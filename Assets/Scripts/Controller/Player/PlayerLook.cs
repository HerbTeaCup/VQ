using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLook : MonoBehaviour, IClassHasChain
{
    PlayerStatus _status;

    [SerializeField] Transform _point;
    [SerializeField] Transform _aiming;

    float maxDis = 6f;

    // Start is called before the first frame update
    void Start()
    {
        GetPoint();
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += PointMove;
        GameManager.Input.InputDelegate += CameraChange;

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
            }
            if (child.name == "AimingPoint")
            {
                _aiming = child;
                return;
            }
        }
    }
    void CameraChange()
    {
        if (GameManager.Input.Aiming) { GameManager.Cam.SetHighestPriority("Player Aiming Cam"); }
        else { GameManager.Cam.SetHighestPriority("Player Follow Cam"); }
    }
    void PointMove()
    {
        if(GameManager.Input.Aiming == false) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, _status.GroundLayer))
        {
            float dis = Vector3.Distance(this.transform.position, new Vector3(hit.point.x, this.transform.position.y, hit.point.z));

            if (dis <= maxDis)
            {
                // 오브젝트를 클릭 지점으로 이동
                _point.position = hit.point;
            }
            else
            {
                // 거리가 조건 이상이면, 조건만큼 떨어진 지점을 계산하여 이동
                Vector3 direction = (hit.point - this.transform.position).normalized;
                Vector3 targetPosition = this.transform.position + direction * 8f;
                _point.position = targetPosition;
            }

            _aiming.position = (this.transform.position + _point.position) * 0.4f;
        }

        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }
}
