using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager
{
    public System.Action PuzzleDelegate = null;

    public void PuzzleUpdate()
    {
        if (PuzzleDelegate != null)
        {
            PuzzleDelegate();
        }
    }
}
