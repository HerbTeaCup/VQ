using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    InputManager _input = null;

    PuzzleManager _puzzle = new PuzzleManager();
    CameraManager _cam = new CameraManager();

    GameObject _player;

    //Property
    public static GameManager Instance { get { Init(); return _instance; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PuzzleManager Puzzle { get { return Instance._puzzle; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static GameObject Player { get { return Instance._player; } set { Instance._player = value; } }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Input.Updater();
        Puzzle.PuzzleUpdate();
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
