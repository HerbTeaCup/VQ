using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    InputManager _input = null;

    CameraManager _cam = new CameraManager();

    //Property
    public static GameManager Instance { get { Init(); return _instance; } }
    public static InputManager Input { get { return Instance._input; } }
    public static CameraManager Cam { get { return Instance._cam; } }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Input.Updater();
    }
    private void LateUpdate()
    {
        Input.LateUpdater();
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject temp = GameObject.Find("@GameManager");

            if (temp == null)
            {
                temp = new GameObject("@GameManager");
            }

            temp.TryGetComponent<GameManager>(out _instance);
            if (_instance == null) { _instance = temp.AddComponent<GameManager>(); }

            temp.TryGetComponent<InputManager>(out _instance._input);
            if (_instance._input == null) { _instance._input = temp.AddComponent<InputManager>(); }
        }
    }
}
