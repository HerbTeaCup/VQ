using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableClass : MonoBehaviour, IPuzzleInteraction
{
    public InteracitveObjType type;
    protected bool ready = true;

    protected void ReadyCheck()
    {
        StopCoroutine(Waiting());
        StartCoroutine(Waiting());
    }
    public virtual void ElementHit(ElementType type)
    {
        ReadyCheck();

        Debug.Log("ElementHit Logic");
    }

    public virtual void Interactive()
    {
        ReadyCheck();

        Debug.Log("Interactive Logic");
    }

    protected IEnumerator Waiting(float duration = 0.7f)
    {
        ready = false;
        yield return new WaitForSeconds(duration);
        ready = true;
    }
}