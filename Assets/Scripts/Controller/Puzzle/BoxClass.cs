using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxClass : InteractableClass, IPuzzleBox
{
    [SerializeField] BoxType cubeType;
    public bool pushable { get; private set; }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        type = InteracitveObjType.Box;

        if (cubeType != BoxType.Magma)
        {
            pushable = true;
        }
        else
        {
            pushable = false;
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
        if (!pushable) return;

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

        Vector3 closestDirection = Vector3.zero;
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

        if (Vector3.Dot(closestDirection, directionToPlayer) < 0.766f) { return; }

        // 박스를 가장 가까운 방향으로 밀어 이동합니다.
        Vector3 moveDirection = -closestDirection;
        Vector3 newPosition = boxPosition + moveDirection;

        // 이동 가능한지 여부를 검사합니다.
        if (IsValidPosition(newPosition))
        {
            transform.position = newPosition;
        }
        
    }
    bool IsValidPosition(Vector3 position)
    {
        // 박스가 이동할 수 있는지 확인합니다.
        // 예를 들어, 충돌 검사를 추가할 수 있습니다.
        // 이 예제에서는 기본적으로 박스가 자유롭게 이동할 수 있다고 가정합니다.
        return true;
    }
}