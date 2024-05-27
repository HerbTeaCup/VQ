using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCameraBase CamBase;

    void Start()
    {
        CamBase = GetComponent<CinemachineVirtualCameraBase>();

        GameManager.Cam.AddCamera($"{this.gameObject.name}", CamBase);
    }
}
