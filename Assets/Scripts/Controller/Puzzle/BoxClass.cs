using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxClass : InteractableClass, IPuzzleBox
{
    [SerializeField] BoxType cubeType;
    public bool pushable { get; private set; }

    Vector3 closestDirection = Vector3.zero;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        type = InteracitveObjType.Box;

        if (cubeType == BoxType.Magma)
        {
            pushable = false;
        }
        else
        {
            pushable = true;
        }
    }

    public override void ElementHit(ElementType type)
    {
        base.ElementHit(type);
    }
    public override void Interactive()
    {
        base.Interactive();
        Push();
    }

    void Push()
    {
        if(pushable == false) { return; } //혹시모르니 한번  더 체크

        // 박스를 가장 가까운 방향으로 밀어 이동합니다.
        Vector3 moveDirection = -closestDirection;
        Vector3 newPosition = transform.position + moveDirection;

        // 이동 가능한지 여부를 검사합니다.
        if (IsValidPosition(newPosition))
        {
            transform.position = newPosition;
        }
    }
    bool IsValidPosition(Vector3 position)
    {
        // 박스가 이동할 수 있는지 확인
        return true;
    }

    public bool CheckAngle()
    {
        if (!pushable) { return false; }

        Vector3 boxPosition = transform.position;
        Vector3 playerPosition = GameManager.Player.transform.position;
        Vector3 directionToPlayer = (playerPosition - boxPosition).normalized;

        // 상하좌우 방향을 정의합니다.
        Vector3[] possibleDirections =
        {
            transform.forward,  // 박스의 앞 방향
            -transform.forward, // 박스의 뒤 방향
            transform.right,    // 박스의 오른쪽 방향
            -transform.right    // 박스의 왼쪽 방향
        };

        float closestDotProduct = -1f; // 가장 가까운 방향의 Dot Product 초기값

        // 플레이어 방향과 박스 방향 간의 가장 가까운 방향을 찾습니다.
        foreach (var direction in possibleDirections)
        {
            float dotProduct = Vector3.Dot(direction, directionToPlayer);
            if (dotProduct > closestDotProduct)
            {
                closestDotProduct = dotProduct;
                closestDirection = direction;
            }
        }

        return Vector3.Dot(closestDirection, directionToPlayer) > 0.766f;
    }
}