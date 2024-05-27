using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] LayerMask GroundLayer;
    public float currentSpeed = 0f;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    [Tooltip("속도 변화 정도")] [Range(5, 15)] public float speedRatio = 10f;
    [Tooltip("각도 변화 정도")] [Range(5, 15)] public float rotateRatio = 10f;
    public bool isGrounded = false;

    private void Start()
    {
        GameManager.Input.InputDelegate += GroundCheck;
    }

    void GroundCheck()
    {
        float radius = 0.2f;

        Vector3 checkPosition = this.transform.position + new Vector3(0, radius - 0.05f, 0);
        isGrounded = Physics.CheckSphere(checkPosition, radius, GroundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;

        float radius = 0.2f;
        Vector3 checkPosition = this.transform.position + new Vector3(0, radius - 0.05f, 0);

        Gizmos.DrawSphere(checkPosition, radius);
    }
}
