using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalInteractive : MonoBehaviour
{
    [SerializeField] ElementType type;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("InteractiveElement") == false) { return; }

        IPuzzleInteraction temp = collision.transform.GetComponent<IPuzzleInteraction>();
        temp.ElementHit(type);
    }
}
