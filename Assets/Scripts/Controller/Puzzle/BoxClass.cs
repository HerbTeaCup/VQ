using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxClass : InteractableClass, IPuzzleBox
{
    [SerializeField] BoxType cubeType;
    public bool pushable { get; private set; }

    Vector3 _pushDir;

    private void Start()
    {
        Init();

        GameManager.Puzzle.PuzzleDelegate += Searching;
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
    void Searching()
    {
        if (cubeType == BoxType.Magma) { pushable = false; return; }

        Collider[] player = Physics.OverlapSphere(this.transform.position, 1f, 1 << 6);
        pushable = player.Length > 0;
    }

    void Push()
    {
        if (!pushable) return;

        Vector3 boxPosition = transform.position;
        Vector3 playerPosition = GameManager.Player.transform.position;
        Vector3 directionToPlayer = (playerPosition - boxPosition).normalized;

        // �����¿� ������ �����մϴ�.
        Vector3[] possibleDirections =
        {
            transform.forward,  // �ڽ��� �� ����
            -transform.forward, // �ڽ��� �� ����
            transform.right,    // �ڽ��� ������ ����
            -transform.right    // �ڽ��� ���� ����
        };

        Vector3 closestDirection = Vector3.zero;
        float closestDotProduct = -1f; // ���� ����� ������ Dot Product �ʱⰪ

        // �÷��̾� ����� �ڽ� ���� ���� ���� ����� ������ ã���ϴ�.
        foreach (var direction in possibleDirections)
        {
            float dotProduct = Vector3.Dot(direction, directionToPlayer);
            if (dotProduct > closestDotProduct)
            {
                closestDotProduct = dotProduct;
                closestDirection = direction;
            }
        }

        // �ڽ��� ���� ����� �������� �о� �̵��մϴ�.
        Vector3 moveDirection = -closestDirection;
        Vector3 newPosition = boxPosition + moveDirection;

        // �̵� �������� ���θ� �˻��մϴ�.
        if (IsValidPosition(newPosition))
        {
            transform.position = newPosition;
        }
        
    }
    bool IsValidPosition(Vector3 position)
    {
        // �ڽ��� �̵��� �� �ִ��� Ȯ���մϴ�.
        // ���� ���, �浹 �˻縦 �߰��� �� �ֽ��ϴ�.
        // �� ���������� �⺻������ �ڽ��� �����Ӱ� �̵��� �� �ִٰ� �����մϴ�.
        return true;
    }
}